using Fusion;
using Fusion.XR.Host.Grabbing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public partial class Define
{
    [System.Serializable]
    public struct HandCommand2 : INetworkStruct
    {
        public bool LearFarInteractor_SelectActivate;
        public float LearFarInteractor_SelectValue;

        public float ActivateValue;
    }

    // Structure representing the inputs driving a hand pose 
    [System.Serializable]
    public struct HandCommand : INetworkStruct
    {
        public float thumbTouchedCommand;
        public float indexTouchedCommand;
        public float gripCommand;
        public float triggerCommand;
        // Optionnal commands
        public int poseCommand;
        public float pinchCommand;// Can be computed from triggerCommand by default
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
        public HandCommand2 leftHandCommand;
        public HandCommand2 rightHandCommand;
        public GrabInfo leftGrabInfo;
        public GrabInfo rightGrabInfo;
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
