using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class InputHandler : MonoBehaviour
{
    [Header("Ref")]
    public @XRIDefaultInputActions inputActions;
    PlayerManager m_PlayerManager;
    GestureEventProcessor m_GestureEventProcessor;


    [Header("XRI Right")]
    public bool right_select;
    public bool right_Trigger;

    [Header("XRI Left")]
    public bool left_select;
    public bool left_Trigger;

    [Header("Keyboard")]
    public bool InstantlySpelluse;

    private void Start()
    {
        m_PlayerManager = GetComponent<PlayerManager>();
        m_GestureEventProcessor = GetComponent<GestureEventProcessor>();
    }

    private void OnEnable()
    {
        if(inputActions == null)
        {
            inputActions = new XRIDefaultInputActions();

            // Right
            inputActions.XRIRightInteraction.Select.performed += i => right_select = true;
            inputActions.XRIRightInteraction.Activate.performed += i => right_Trigger = true;
            inputActions.XRIRightInteraction.Activate.canceled += i => right_Trigger = false;

            // Left
            inputActions.XRILeftInteraction.Select.performed += i => left_Trigger = true;
            inputActions.XRILeftInteraction.Activate.performed += i => left_Trigger = true;
            inputActions.XRILeftInteraction.Activate.canceled += i => left_Trigger = false;

            // Keyboard
            inputActions.Keyboard.InstantlySpellUse.performed += i => InstantlySpelluse = true;
        }

        inputActions.Enable();
    }

    private void InstantlySpellUse_performed(CallbackContext obj)
    {
        throw new System.NotImplementedException();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        GestureRecognitionInput();
        TestDevelupSpell();
    }
    
    private void LateUpdate()
    {
        
    }

    public void GestureRecognitionInput()
    {
        // Right Trigger Logic
        if (right_Trigger)
        {
            FirstInputHandle(); // 첫 입력 처리
            m_PlayerManager.MoveMagicStaff(true); // 파티클 생성
        }
        else
        {
            m_PlayerManager.MoveMagicStaff(false); // 파티클 제거
        }

        // Left Trigger Logic
        //if (left_Trigger)
        //{
        //    FirstInputHandle(); // 첫 입력 처리
        //    m_PlayerManager.MoveMagicStaff(true); // 파티클 생성
        //}
        //else
        //{
        //    m_PlayerManager.MoveMagicStaff(false); // 파티클 제거
        //}
    }

    private void FirstInputHandle()
    {
        if (m_PlayerManager.m_GestureEventProcessor.m_bIsFirstRecognition == false)
        {
            m_PlayerManager.m_GestureEventProcessor.m_bIsFirstRecognition = true;
            m_GestureEventProcessor.m_Mivry.enabled = true;
        }
    }

    // 조건 없이 스펠을 바로 쓸 수 있게 해주는 거
    private void TestDevelupSpell()
    {
        if(left_select || InstantlySpelluse)
        {
            InstantlySpelluse = false;
            left_select = false;
            m_PlayerManager.m_PlayerMagicManager.SimpleAttempSpell();
        }
    }
}
