using UnityEngine;

public class ConnectedState : IState
{
    public void HandleInput(ModuleStateController stateController, ModuleInput input)
    {
        if (input.isLeftMouseDown && input.isMouseOver)
        {
            stateController.ChangeState(stateController.PlacingState);
        }

        if (input.isMouseOver && input.isRightMouseDown)
        {
            GameObject.Destroy(stateController.gameObject);
            return;
        }
    }

    public void OnEnter(ModuleStateController stateController, IState previousState)
    {
        //Debug.Log("[ConnectedState] OnEnter!");
        DraggableModule module = stateController.gameObject.GetComponent<DraggableModule>();
        module.PlaceModule();
    }

    public void OnExit(ModuleStateController stateController)
    {
        //Debug.Log("[ConnectedState] OnExit!");
    }

    public void UpdateState(ModuleStateController stateController)
    {
    }
}
