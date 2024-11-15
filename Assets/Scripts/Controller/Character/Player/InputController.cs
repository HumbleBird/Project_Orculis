using UnityEngine;

public class InputHandler : MonoBehaviour
{
    @XRIDefaultInputActions inputActions;

    private void OnEnable()
    {
        if(inputActions == null)
            inputActions = new XRIDefaultInputActions();

        inputActions.Enable();
    }

    private void OnDisable()
    {
        if (inputActions != null)
            inputActions.Disable();

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
