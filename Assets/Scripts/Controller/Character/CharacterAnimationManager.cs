using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterAnimationManager : MonoBehaviour
{
    void Start()
    {
        ReStartAnimator(m_LeftHandAnimator);
        ReStartAnimator(m_RightHandAnimator);
    }

    private void Update()
    {
        HandAnimation(m_LeftHandAnimator, m_LeftgripAnimationAction, m_LeftpinchAnimationAction);
        HandAnimation(m_RightHandAnimator, m_RightgripAnimationAction, m_RightpinchAnimationAction);
    }

    // Event function
    void LateUpdate()
    {
        MappingHandTransform(leftHandIK, leftHandController, true);
        MappingHandTransform(rightHandIK, rightHandController, false);
        MappingBodyTransform(headIK, hmd);
        MappingHeadTransform(headIK, hmd);
    }

    #region Rigging

    public Transform leftHandIK;
    public Transform rightHandIK;
    public Transform headIK;

    public Transform leftHandController;
    public Transform rightHandController;
    public Transform hmd;

    public Vector3[] leftOffset;   // 0: Position, 1: Rotation
    public Vector3[] rightOffset;
    public Vector3[] headOffset;

    public float smoothValue = 0.1f;
    public float modelHeight = 1.67f;

    // 오프셋 추가 (180도 보정)
    public Vector3 bodyRotationOffset = new Vector3(0, 180, 0);
    public Vector3 headRotationOffset = new Vector3(0, 180, 0);

    public Transform m_ModelTransform;



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
