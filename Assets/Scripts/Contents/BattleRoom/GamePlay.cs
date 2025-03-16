using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Fusion;
using static Define;

#if !UNITY_EDITOR && (UNITY_WEBGL || UNITY_ANDROID || UNITY_IOS)
#error This sample doesn't support currently selected platform, please switch to Windows, Mac, Linux in Build Settings.
#endif

namespace SimpleFPS
{
    /// <summary>
    /// Drives gameplay logic - state, timing, handles player connect/disconnect/spawn/despawn/death, calculates statistics.
    /// </summary>
    public class Gameplay : NetworkBehaviour
    {
        //public GameUI GameUI;
        public GameObject PlayerPrefab;
        public float GameDuration = 180f;
        public float PlayerRespawnTime = 5f;
        public float DoubleDamageDuration = 30f;
        public GameObject m_HardwareRig;

        [Networked][Capacity(32)][HideInInspector]
		public NetworkDictionary<PlayerRef, PlayerData> PlayerData { get; }
		[Networked][HideInInspector]
		public TickTimer RemainingTime { get; set; }
		[Networked][HideInInspector]
		public EGameplayState State { get; set; }

        private bool _isNicknameSent;
        private float _runningStateTime;
        private List<Player> _spawnedPlayers = new(16);
        private List<PlayerRef> _pendingPlayers = new(16);
        private List<PlayerData> _tempPlayerData = new(16);
        private List<Transform> _recentSpawnPoints = new(4);

        public void PlayerKilled(PlayerRef killerPlayerRef, PlayerRef victimPlayerRef, E_WeaponType weaponType)
        {
            if (HasStateAuthority == false)
                return;

            // Update statistics of the killer player.
            if (PlayerData.TryGet(killerPlayerRef, out PlayerData killerData))
            {
                killerData.Kills++;
                killerData.LastKillTick = Runner.Tick;
                PlayerData.Set(killerPlayerRef, killerData);
            }

            // Update statistics of the victim player.
            var playerData = PlayerData.Get(victimPlayerRef);
            playerData.Deaths++;
            playerData.IsAlive = false;
            PlayerData.Set(victimPlayerRef, playerData);

            // Inform all clients about the kill via RPC.
            RPC_PlayerKilled(killerPlayerRef, victimPlayerRef, weaponType);

            StartCoroutine(RespawnPlayer(victimPlayerRef, PlayerRespawnTime));

            RecalculateStatisticPositions();
        }

        public override void Spawned()
        {
            if (Runner.Mode == SimulationModes.Server)
            {
                Application.targetFrameRate = TickRate.Resolve(Runner.Config.Simulation.TickRateSelection).Server;
            }

            if (Runner.GameMode == GameMode.Shared)
            {
                throw new System.NotSupportedException("This sample doesn't support Shared Mode, please start the game as Server, Host or Client.");
            }
        }

        public override void FixedUpdateNetwork()
        {
            if (HasStateAuthority == false)
                return;

            // PlayerManager is a special helper class which iterates over list of active players (NetworkRunner.ActivePlayers) and call spawn/despawn callbacks on demand.
            PlayerManager.UpdatePlayerConnections(Runner, SpawnPlayer, DespawnPlayer);

            // Start gameplay when there are enough players connected.
            if (State == EGameplayState.Skirmish && PlayerData.Count > 1)
            {
                StartGameplay();
            }

            if (State == EGameplayState.Running)
            {
                _runningStateTime += Runner.DeltaTime;

                var sessionInfo = Runner.SessionInfo;

                // Hide the match after 60 seconds. Players won't be able to randomly connect to existing game and start new one instead.
                // Joining via party code should work.
                if (sessionInfo.IsVisible && (_runningStateTime > 60f || sessionInfo.PlayerCount >= sessionInfo.MaxPlayers))
                {
                    sessionInfo.IsVisible = false;
                }

                if (RemainingTime.Expired(Runner))
                {
                    StopGameplay();
                }
            }
        }

        public override void Render()
        {
            if (Runner.Mode == SimulationModes.Server)
                return;

            // Every client must send its nickname to the server when the game is started.
            if (_isNicknameSent == false)
            {
                RPC_SetPlayerNickname(Runner.LocalPlayer, PlayerPrefs.GetString("Photon.Menu.Username"));
                _isNicknameSent = true;
            }
        }

        # region Spawn Respawn Despawn

        private void SpawnPlayer(PlayerRef playerRef)
        {
            if (PlayerData.TryGet(playerRef, out var playerData) == false)
            {
                playerData = new PlayerData();
                playerData.PlayerRef = playerRef;
                playerData.Nickname = playerRef.ToString();
                playerData.StatisticPosition = int.MaxValue;
                playerData.IsAlive = false;
                playerData.IsConnected = false;
            }

            if (playerData.IsConnected == true)
                return;

            Debug.LogWarning($"{playerRef} connected.");

            playerData.IsConnected = true;
            playerData.IsAlive = true;

            PlayerData.Set(playerRef, playerData);

            var spawnPoint = GetSpawnPoint();
            var player = Runner.Spawn(PlayerPrefab, spawnPoint.position, spawnPoint.rotation, playerRef);

            // Set player instance as PlayerObject so we can easily get it from other locations.
            Runner.SetPlayerObject(playerRef, player);

            // hard rig
            if (player.HasInputAuthority)
            {
                m_HardwareRig.transform.position = spawnPoint.position;
            }

            RecalculateStatisticPositions();
        }

        private void DespawnPlayer(PlayerRef playerRef, Player player)
        {
            if (PlayerData.TryGet(playerRef, out var playerData) == true)
            {
                if (playerData.IsConnected == true)
                {
                    Debug.LogWarning($"{playerRef} disconnected.");
                }

                playerData.IsConnected = false;
                playerData.IsAlive = false;
                PlayerData.Set(playerRef, playerData);
            }

            Runner.Despawn(player.Object);

            RecalculateStatisticPositions();
        }

        private IEnumerator RespawnPlayer(PlayerRef playerRef, float delay)
        {
            if (delay > 0f)
                yield return new WaitForSecondsRealtime(delay);

            if (Runner == null)
                yield break;

            // Despawn old player object if it exists.
            var playerObject = Runner.GetPlayerObject(playerRef);
            if (playerObject != null)
            {
                Runner.Despawn(playerObject);
            }

            // Don't spawn the player for disconnected clients.
            if (PlayerData.TryGet(playerRef, out PlayerData playerData) == false || playerData.IsConnected == false)
                yield break;

            // Update player data.
            playerData.IsAlive = true;
            PlayerData.Set(playerRef, playerData);

            var spawnPoint = GetSpawnPoint();
            var player = Runner.Spawn(PlayerPrefab, spawnPoint.position, spawnPoint.rotation, playerRef);

            // hard rig
            if (player.HasInputAuthority)
            {
                m_HardwareRig.transform.position = spawnPoint.position;
            }


            // Set player instance as PlayerObject so we can easily get it from other locations.
            Runner.SetPlayerObject(playerRef, player);
        }

        private Transform GetSpawnPoint()
        {
            Transform spawnPoint = default;

            // Iterate over all spawn points in the scene.
            var spawnPoints = Runner.SimulationUnityScene.GetComponents<SpawnPoint>(false);
            for (int i = 0, offset = Random.Range(0, spawnPoints.Length); i < spawnPoints.Length; i++)
            {
                spawnPoint = spawnPoints[(offset + i) % spawnPoints.Length].transform;

                if (_recentSpawnPoints.Contains(spawnPoint) == false)
                    break;
            }

            // Add spawn point to list of recently used spawn points.
            _recentSpawnPoints.Add(spawnPoint);

            // Ignore only last 3 spawn points.
            //if (_recentSpawnPoints.Count > 3)
            //{
            //    _recentSpawnPoints.RemoveAt(0);
            //}

            return spawnPoint;
        }

        #endregion

        private void StartGameplay()
        {
            // Stop all respawn coroutines.
            StopAllCoroutines();

            State = EGameplayState.Running;
            RemainingTime = TickTimer.CreateFromSeconds(Runner, GameDuration);

            // Reset player data after skirmish and respawn players.
            foreach (var playerPair in PlayerData)
            {
                var data = playerPair.Value;

                data.Kills = 0;
                data.Deaths = 0;
                data.StatisticPosition = int.MaxValue;
                data.IsAlive = false;

                PlayerData.Set(data.PlayerRef, data);

                StartCoroutine(RespawnPlayer(data.PlayerRef, 0f));
            }
        }

        private void StopGameplay()
        {
            RecalculateStatisticPositions();

            State = EGameplayState.Finished;
        }

        private void RecalculateStatisticPositions()
        {
            if (State == EGameplayState.Finished)
                return;

            _tempPlayerData.Clear();

            foreach (var pair in PlayerData)
            {
                _tempPlayerData.Add(pair.Value);
            }

            _tempPlayerData.Sort((a, b) =>
            {
                if (a.Kills != b.Kills)
                    return b.Kills.CompareTo(a.Kills);

                return a.LastKillTick.CompareTo(b.LastKillTick);
            });

            for (int i = 0; i < _tempPlayerData.Count; i++)
            {
                var playerData = _tempPlayerData[i];
                playerData.StatisticPosition = playerData.Kills > 0 ? i + 1 : int.MaxValue;

                PlayerData.Set(playerData.PlayerRef, playerData);
            }
        }


        #region RPC



        [Rpc(RpcSources.StateAuthority, RpcTargets.All, Channel = RpcChannel.Reliable)]
        private void RPC_PlayerKilled(PlayerRef killerPlayerRef, PlayerRef victimPlayerRef, E_WeaponType weaponType)
        {
            string killerNickname = "";
            string victimNickname = "";

            if (PlayerData.TryGet(killerPlayerRef, out PlayerData killerData))
            {
                killerNickname = killerData.Nickname;
            }

            if (PlayerData.TryGet(victimPlayerRef, out PlayerData victimData))
            {
                victimNickname = victimData.Nickname;
            }

            //foreach (var player in _spawnedPlayers)
                //player.m_HUD.m_KillFeed.ShowKill(killerNickname, victimNickname, weaponType);
        }

        [Rpc(RpcSources.All, RpcTargets.StateAuthority, Channel = RpcChannel.Reliable)]
        private void RPC_SetPlayerNickname(PlayerRef playerRef, string nickname)
        {
            var playerData = PlayerData.Get(playerRef);
            playerData.Nickname = nickname;
            PlayerData.Set(playerRef, playerData);
        }

        [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority, Channel = RpcChannel.Reliable, HostMode = RpcHostMode.SourceIsHostPlayer)]
        public void RPC_ClientRequestHP(Player attacker, Player defender, int damage)
        {

            Debug.Log("RPC_ClientRequestHP");
            defender.m_PlayerStatesManager.m_CurrentHealth = Mathf.Clamp(defender.m_PlayerStatesManager.m_CurrentHealth - damage, 0, defender.m_PlayerStatesManager.m_MaxHealth);
            if (defender.m_PlayerStatesManager.m_CurrentHealth <= 0)
            {
                Debug.Log($"플레이어 사망 {defender.name}");
                // OnPlayerDeath();
            }
        }

        public void ClientChangeHp(Player attacker, Player defender, int damage)
        {
            if (HasStateAuthority == false)
                return;

            Debug.Log("RPC_ClientRequestHP");
            defender.m_PlayerStatesManager.m_CurrentHealth = Mathf.Clamp(defender.m_PlayerStatesManager.m_CurrentHealth - damage, 0, defender.m_PlayerStatesManager.m_MaxHealth);
            if (defender.m_PlayerStatesManager.m_CurrentHealth <= 0)
            {
                Debug.Log($"플레이어 사망 {defender.name}");
                // OnPlayerDeath();
            }
        }

        #endregion
    }
}
