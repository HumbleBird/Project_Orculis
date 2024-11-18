using UnityEngine;

public class InputHandler : MonoBehaviour
{
    [Header("Ref")]
    @XRIDefaultInputActions inputActions;
    PlayerManager m_PlayerManager;


    [Header("XRI Right")]
    public bool right_select;


    private void Start()
    {
        m_PlayerManager = GetComponent<PlayerManager>();
    }

    private void OnEnable()
    {
        if(inputActions == null)
        {
            inputActions = new XRIDefaultInputActions();


            inputActions.XRIRightInteraction.Select.performed += i => right_select = true;



        }

        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    
    private void LateUpdate()
    {
        
    }
}
