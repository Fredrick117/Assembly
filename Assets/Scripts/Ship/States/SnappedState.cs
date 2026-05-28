using UnityEngine;

public class SnappedState : IState
{
    public void HandleInput(ModuleStateController stateController, ModuleInput input)
    {
        if (input.isLeftMouseDown)
        {
            stateController.ChangeState(stateController.ConnectedState);
        }
    }

    public void OnEnter(ModuleStateController stateController)
    {
        Debug.Log("[SnappedState] OnEnter!");

        stateController.GetComponent<DraggableModule>().ChangePlacementColor(true);
    }

    public void OnExit(ModuleStateController stateController)
    {
        Debug.Log("[SnappedState] OnExit!");
        stateController.GetComponent<DraggableModule>().ChangePlacementColor(false);
    }

    public void UpdateState(ModuleStateController stateController)
    {
        DraggableModule module = stateController.gameObject.GetComponent<DraggableModule>();

        Vector2 mousePosition = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float distance = Vector2.Distance(stateController.gameObject.transform.position, mousePosition);

        if (distance > module.mouseUnsnapDistance)
        {
            stateController.ChangeState(stateController.PlacingState);
        }

        return;
    }
}
