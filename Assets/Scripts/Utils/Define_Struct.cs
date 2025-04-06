using Fusion;
using UnityEngine;

public partial class Define
{
    [System.Serializable]
    public struct HandCommand : INetworkStruct
    {
        public bool LearFarInteractor_SelectActivate;
        public float LearFarInteractor_SelectValue;

        public bool Activate;
        public float ActivateValue;
    }

    [System.Serializable]
    public struct RigInput : INetworkInput
    {
        public Vector3 playAreaPosition;
        public Quaternion playAreaRotation;
        public Vector3 leftHandPosition;
        public Quaternion leftHandRotation;
        public Vector3 rightHandPosition;
        public Quaternion rightHandRotation;
        public Vector3 headsetPosition;
        public Quaternion headsetRotation;
        public HandCommand leftHandCommand;
        public HandCommand rightHandCommand;

        public NetworkBool Alpha0;
        public NetworkBool Alpha1;
        public NetworkBool Alpha2;
        public NetworkBool Alpha3;
        public NetworkBool Alpha4;
        public NetworkBool Alpha5;
        public NetworkBool Alpha6;
        public NetworkBool Alpha7;
        public NetworkBool Alpha8;
        public NetworkBool Alpha9;
    }

    public struct PlayerData : INetworkStruct
    {
        [Networked, Capacity(24)]
        public string Nickname { get => default; set { } }
        public PlayerRef PlayerRef;
        public int Kills;
        public int Deaths;
        public int LastKillTick;
        public int StatisticPosition;
        public bool IsAlive;
        public bool IsConnected;
    }
}
