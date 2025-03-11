using Fusion;
using Fusion.XR.Host.Rig;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static Define;

// Transform도 포함
[RequireComponent(typeof(NetworkTransform))]
public class CharacterAnimationManager : NetworkBehaviour
{
    [Header("Ref")]
    public Player m_Player;

    [Header("IK")]
    public Transform leftHandIK;
    public Transform rightHandIK;
    public Transform headIK;

    [Header("Controller")]
    public Transform leftHandController;
    public Transform rightHandController;
    public Transform HeadController;

    [Header("Offset")]
    public Vector3[] leftOffset;   // 0: Position, 1: Rotation
    public Vector3[] rightOffset;
    public Vector3[] headOffset;

    public float smoothValue = 0.1f;
    public float modelHeight = 1.67f;

    // 오프셋 추가 (180도 보정)
    public Vector3 bodyRotationOffset = new Vector3(0, 180, 0);
    public Vector3 headRotationOffset = new Vector3(0, 180, 0);

    public Transform m_ModelTransform;

    [Header("Character Model")]
    public List<GameObject> m_HeadObjectModel = new List<GameObject>();

    public override void Spawned()
    {
        m_Player = GetComponent<Player>();

        SetCharacterHeadLayer();

        ReStartAnimator(m_LeftHandAnimator);
        ReStartAnimator(m_RightHandAnimator);
    }

    public override void FixedUpdateNetwork()
    {
        base.FixedUpdateNetwork();

        // update the rig at each network tick
        if (GetInput<RigInput>(out var input))
        {
            //transform.position = input.playAreaPosition;
            //transform.rotation = input.playAreaRotation;
            leftHandController.position = input.leftHandPosition;
            leftHandController.rotation = input.leftHandRotation;

            rightHandController.position = input.rightHandPosition;
            rightHandController.rotation = input.rightHandRotation;

            HeadController.position = input.headsetPosition;
            HeadController.rotation = input.headsetRotation;

        }

        NFixedUpdateNetwork();
    }


    public void NFixedUpdateNetwork()
    {
        HandAnimation(m_LeftHandAnimator, m_LeftgripAnimationAction, m_LeftpinchAnimationAction);
        HandAnimation(m_RightHandAnimator, m_RightgripAnimationAction, m_RightpinchAnimationAction);


        MappingHandTransform(leftHandIK, leftHandController, true);
        MappingHandTransform(rightHandIK, rightHandController, false);
        MappingBodyTransform(headIK, HeadController);
        MappingHeadTransform(headIK, HeadController);
    }

    public override void Render()
    {
        if (HasInputAuthority)
        {
            var hardwareRig = m_Player.m_HardwareRig;

            // 🎯 최신 하드웨어 트래킹 위치로 보정 (시각적으로 부드러운 보정)
            //transform.position = Vector3.Lerp(transform.position, hardwareRig.transform.position, Time.deltaTime * 10);
            //transform.rotation = Quaternion.Slerp(transform.rotation, hardwareRig.transform.rotation, Time.deltaTime * 10);

            //leftHandController.position = Vector3.Lerp(leftHandController.position, hardwareRig.m_LeftHandTransform.position, Time.deltaTime * 10);
            //leftHandController.rotation = Quaternion.Slerp(leftHandController.rotation, hardwareRig.m_LeftHandTransform.rotation, Time.deltaTime * 10);
            //
            //rightHandController.position = Vector3.Lerp(rightHandController.position, hardwareRig.m_RightHandTransform.position, Time.deltaTime * 10);
            //rightHandController.rotation = Quaternion.Slerp(rightHandController.rotation, hardwareRig.m_RightHandTransform.rotation, Time.deltaTime * 10);
            //
            //HeadController.position = Vector3.Lerp(HeadController.position, hardwareRig.m_Headset.position, Time.deltaTime * 10);
            //HeadController.rotation = Quaternion.Slerp(HeadController.rotation, hardwareRig.m_Headset.rotation, Time.deltaTime * 10);

            //leftHandController.position = hardwareRig.m_LeftHandTransform.position;
            //leftHandController.rotation = hardwareRig.m_LeftHandTransform.rotation;
            //
            //rightHandController.position = hardwareRig.m_RightHandTransform.position;
            //rightHandController.rotation = hardwareRig.m_RightHandTransform.rotation;
            //
            //HeadController.position = hardwareRig.m_Headset.position;
            //HeadController.rotation = hardwareRig.m_Headset.rotation;
        }
        else
        {

        }
    }

    public void LateUpdate()
    {
        if (HasInputAuthority)
        {
            var hardwareRig = m_Player.m_HardwareRig;

            // 🎯 VR 트래킹된 최신 위치 적용 (애니메이션 리깅)
            //transform.position = hardwareRig.transform.position;
            //transform.rotation = hardwareRig.transform.rotation;

            //leftHandController.position = hardwareRig.m_LeftHandTransform.position;
            //leftHandController.rotation = hardwareRig.m_LeftHandTransform.rotation;
            //
            //rightHandController.position = hardwareRig.m_RightHandTransform.position;
            //rightHandController.rotation = hardwareRig.m_RightHandTransform.rotation;
            //
            //HeadController.position = hardwareRig.m_Headset.position;
            //HeadController.rotation = hardwareRig.m_Headset.rotation;
        }
    }



    private void SetCharacterHeadLayer()
    {
        int Head = (LayerMask.NameToLayer("Head"));
        int Default = (LayerMask.NameToLayer("Default"));
        int temp;

        if (HasInputAuthority)
            temp = Head;
        else
            temp = Default;

        foreach (var obj in m_HeadObjectModel)
        {
            obj.layer = temp;
        }
    }

    #region Rigging
    // Frequently called
    private void MappingHandTransform(Transform ik, Transform controller, bool isLeft)
    {
        var offset = isLeft ? leftOffset : rightOffset;

        ik.position = controller.TransformPoint(offset[0]);
        ik.rotation = controller.rotation * Quaternion.Euler(offset[1]);
    }

    private void MappingBodyTransform(Transform ik, Transform hmd)
    {
        // 위치 매핑
        m_ModelTransform.position = new Vector3(hmd.position.x, hmd.position.y - modelHeight, hmd.position.z);

        // 회전 매핑 (180도 보정 적용)
        float yaw = hmd.eulerAngles.y;
        var targetRotation = new Vector3(m_ModelTransform.eulerAngles.x, yaw, m_ModelTransform.eulerAngles.z);
        m_ModelTransform.rotation = Quaternion.Lerp(
            m_ModelTransform.rotation,
            Quaternion.Euler(targetRotation) * Quaternion.Euler(bodyRotationOffset),
            smoothValue
        );
    }

    private void MappingHeadTransform(Transform ik, Transform hmd)
    {
        ik.position = hmd.TransformPoint(headOffset[0]);

        // 회전 매핑 (180도 보정 적용)
        ik.rotation = hmd.rotation * Quaternion.Euler(headOffset[1]) * Quaternion.Euler(headRotationOffset);
    }


    #endregion

    #region Hand Animation

    [Header("Hand Animation")]
    public InputActionProperty m_LeftpinchAnimationAction;
    public InputActionProperty m_LeftgripAnimationAction;

    public InputActionProperty m_RightpinchAnimationAction;
    public InputActionProperty m_RightgripAnimationAction;

    public Animator m_LeftHandAnimator;
    public Animator m_RightHandAnimator;

    // 현재 버그로 Enable를 False -> True로 한 번 바꿔줘야 작동됨
    private void ReStartAnimator(Animator animator)
    {
        animator.enabled = false;
        animator.enabled = true;

    }

    public void HandAnimation(Animator animator, InputActionProperty grip, InputActionProperty trigger)
    {
        float triggerValue = trigger.action.ReadValue<float>();
        animator.SetFloat("Trigger", triggerValue);

        float gripValue = grip.action.ReadValue<float>();
        animator.SetFloat("Grip", gripValue);
    }



    #endregion
}
