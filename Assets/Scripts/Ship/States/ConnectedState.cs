using UnityEngine;

public class ConnectedState : IState
{
    public void HandleInput(ModuleStateController stateController, ModuleInput input)
    {
        if (input.isMouseDown)
        {
            stateController.ChangeState(stateController.PlacingState);
        }
    }

    public void OnEnter(ModuleStateController stateController)
    {
        Debug.Log("[ConnectedState] OnEnter!");
        stateController.gameObject.GetComponent<DraggableModule>().PlaceModule();
    }

    public void OnExit(ModuleStateController stateController)
    {
        Debug.Log("[ConnectedState] OnExit!");
    }

    public void UpdateState(ModuleStateController stateController)
    {
    }
}
