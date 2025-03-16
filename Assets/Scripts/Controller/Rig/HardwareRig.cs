using Fusion;
using Fusion.Sockets;
using SimpleFPS;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using static Define;
using System.Collections;
using Fusion.XR.Host.Rig;
using static Unity.Collections.Unicode;
using System.Threading.Tasks;

public class HardwareRig : MonoBehaviour, INetworkRunnerCallbacks
{
    [Header("XRBaseInteractor")]
    public XRBaseInteractor m_RightHandLearFarInteractor;
    public XRBaseInteractor m_LeftHandLearFarInteractor;

    [Header("Ref")]
    public @XRIDefaultInputActions inputActions;
    [SerializeField] private AudioListener m_AudioListener;

    public Transform m_LeftHandTransform;
    public Transform m_RightHandTransform;
    public Transform m_Headset;
    public NetworkRunner runner;


    public RunnerExpectations runnerExpectations = RunnerExpectations.DetectRunner;

    bool searchingForRunner = false;

    public async Task<NetworkRunner> FindRunner()
    {
        while (searchingForRunner) await Task.Delay(10);
        searchingForRunner = true;
        if (runner == null && runnerExpectations != RunnerExpectations.NoRunner)
        {
            if (runnerExpectations == RunnerExpectations.PresetRunner || NetworkProjectConfig.Global.PeerMode == NetworkProjectConfig.PeerModes.Multiple)
            {
                Debug.LogWarning("Runner has to be set in the inspector to forward the input");
            }
            else
            {
                // Try to detect the runner
                runner = FindFirstObjectByType<NetworkRunner>();
                var searchStart = Time.time;
                while (searchingForRunner && runner == null)
                {
                    if (NetworkRunner.Instances.Count > 0)
                    {
                        runner = NetworkRunner.Instances[0];
                    }
                    if (runner == null)
                    {
                        await System.Threading.Tasks.Task.Delay(10);
                    }
                }
            }
        }
        searchingForRunner = false;
        return runner;
    }



    protected virtual async void Start()
    {
        await FindRunner();
        if (runner)
        {
            runner.AddCallbacks(this);
        }

        if (inputActions == null)
            inputActions = new XRIDefaultInputActions();

        inputActions.Enable();
    }


    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        RigInput rigInput = new RigInput();
        rigInput.playAreaPosition = transform.position;
        rigInput.playAreaRotation = transform.rotation;

        rigInput.headsetPosition = m_Headset.position;
        rigInput.headsetRotation = m_Headset.rotation;

        //right controller
        //m_LeftHandCommand.LearFarInteractor_SelectActivate = inputActions.XRILeftInteraction.select.readvalue<bool>();
        rigInput.rightHandPosition = m_RightHandTransform.position;
        rigInput.rightHandRotation = m_RightHandTransform.rotation;
        rigInput.rightHandCommand.LearFarInteractor_SelectValue = inputActions.XRIRightInteraction.SelectValue.ReadValue<float>();
        rigInput.rightHandCommand.ActivateValue = inputActions.XRIRightInteraction.ActivateValue.ReadValue<float>();

        //left controller
        rigInput.leftHandPosition = m_LeftHandTransform.position;
        rigInput.leftHandRotation = m_LeftHandTransform.rotation;


        input.Set(rigInput);
    }

    private void OnDestroy()
    {
        if (searchingForRunner) Debug.LogError("Cancel searching for runner in HardwareRig");
        searchingForRunner = false;
        if (runner) runner.RemoveCallbacks(this);
    }

    #region INetworkRunnerCallbacks (unused)
    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player) { }
    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player) { }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }
    public void OnConnectedToServer(NetworkRunner runner) { }
    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason) { }
    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }
    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }
    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data) { }
    public void OnSceneLoadDone(NetworkRunner runner) { }
    public void OnSceneLoadStart(NetworkRunner runner) { }
    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data) { }
    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress) { }
    #endregion
}
