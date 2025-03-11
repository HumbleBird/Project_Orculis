using Fusion.XR.Host.Grabbing;
using UnityEngine;
using static Define;


namespace Fusion.XR.Host.Rig
{
    /**
     * 
     * Networked VR user
     * 
     * Handle the synchronisation of the various rig parts: headset, left hand, right hand, and playarea (represented here by the NetworkRig)
     * Use the local HardwareRig rig parts position info when this network rig is associated with the local user 
     * 
     * 
     **/

    [RequireComponent(typeof(NetworkTransform))]
    // We ensure to run after the NetworkTransform or NetworkRigidbody, to be able to override the interpolation target behavior in Render()
    [DefaultExecutionOrder(NetworkRig.EXECUTION_ORDER)]
    public class NetworkRig : NetworkBehaviour
    {
        public Player m_Player; 
        public const int EXECUTION_ORDER = 100;
        public HardwareRig23 hardwareRig;
        public Transform m_HeadsetNetworkRig;
        public Transform m_LeftHandNetworkRig;
        public Transform m_RightHandNetworkRig;

        // As we are in host topology, we use the input authority to track which player is the local user
        public bool IsLocalNetworkRig => Object.HasInputAuthority;

        public override void Spawned()
        {
            base.Spawned();
            if (IsLocalNetworkRig)
            {
                hardwareRig = FindFirstObjectByType<HardwareRig23>();
                if (hardwareRig == null) Debug.LogError("Missing HardwareRig in the scene");
            }

            m_Player = GetComponent<Player>();
        }



        public void NRender()
        {
            if (IsLocalNetworkRig)
            {
                // Extrapolate for local user:
                // we want to have the visual at the good position as soon as possible, so we force the visuals to follow the most fresh hardware positions
                // To update the visual object, and not the actual networked position, we move the interpolation targets
                transform.position = hardwareRig.transform.position;
                transform.rotation = hardwareRig.transform.rotation;
                m_LeftHandNetworkRig.transform.position = hardwareRig.m_LeftHandTransform.transform.position;
                m_LeftHandNetworkRig.transform.rotation = hardwareRig.m_LeftHandTransform.transform.rotation;
                m_RightHandNetworkRig.transform.position = hardwareRig.m_RightHandTransform.transform.position;
                m_RightHandNetworkRig.transform.rotation = hardwareRig.m_RightHandTransform.transform.rotation;
                m_HeadsetNetworkRig.transform.position = hardwareRig.m_Headset.transform.position;
                m_HeadsetNetworkRig.transform.rotation = hardwareRig.m_Headset.transform.rotation;
            }
        }
    }
}
