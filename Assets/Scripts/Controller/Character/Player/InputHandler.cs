using Fusion;
using Fusion.Sockets;
using SimpleFPS;
using UnityEngine;
using static Define;

public class InputHandler : NetworkBehaviour, IBeforeUpdate
{
    [Header("Ref")]
    public Player m_Player;
    public @XRIDefaultInputActions inputActions;
    public HandCommand m_LeftHandCommand;
    public HandCommand m_RightHandCommand;

    public override void Spawned()
    {
        if (HasInputAuthority == false)
            return;

        m_Player = GetComponent<Player>();

        // Register to Fusion input poll callback.
        var networkEvents = Runner.GetComponent<NetworkEvents>();
        networkEvents.OnInput.AddListener(OnInput);

        if (inputActions == null)
            inputActions = new XRIDefaultInputActions();

        inputActions.Enable();
    }


    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        RigInput rigInput = new RigInput();
        rigInput.playAreaPosition = transform.position;
        rigInput.playAreaRotation = transform.rotation;

        // Left Controller
        //m_LeftHandCommand.LearFarInteractor_SelectActivate = inputActions.XRILeftInteraction.Select.ReadValue<bool>();
        rigInput.rightHandCommand.LearFarInteractor_SelectValue = inputActions.XRIRightInteraction.SelectValue.ReadValue<float>();
        rigInput.rightHandCommand.ActivateValue = inputActions.XRIRightInteraction.ActivateValue.ReadValue<float>();

        // Right Controller
        //m_RightHandCommand.LearFarInteractor_SelectActivate = inputActions.XRIRightInteraction.Select.ReadValue<bool>();
        //rigInput.rightHandCommand.LearFarInteractor_SelectValue = inputActions.XRIRightInteraction.SelectValue.ReadValue<float>();


        input.Set(rigInput);
    }


	public override void Despawned(NetworkRunner runner, bool hasState)
	{
		if (runner == null)
            return;

        var networkEvents = runner.GetComponent<NetworkEvents>();
        if (networkEvents != null)
        {
            networkEvents.OnInput.RemoveListener(OnInput);
        }
    }

    void IBeforeUpdate.BeforeUpdate()
    {
        // This method is called BEFORE ANY FixedUpdateNetwork() and is used to accumulate input from Keyboard/Mouse.
        // Input accumulation is mandatory - this method is called multiple times before new forward FixedUpdateNetwork() - common if rendering speed is faster than Fusion simulation.

        if (HasInputAuthority == false)
            return;


    }
}
