using UnityEngine;

public class PlacingState : IState
{
    public void HandleInput(ModuleStateController stateController, ModuleInput input)
    {
        if (input.isMouseDown)
        {
            Debug.Log("[PlacingState] HandleInput");
            stateController.ChangeState(stateController.ConnectedState);
        }
    }

    public void OnEnter(ModuleStateController stateController)
    {
        Debug.Log("[PlacingState] OnEnter!");
        stateController.gameObject.GetComponent<DraggableModule>().PickUpModule();
    }

    public void OnExit(ModuleStateController stateController)
    {
        Debug.Log("[PlacingState] OnExit!");
    }

    public void UpdateState(ModuleStateController stateController)
    {
        DraggableModule module = stateController.gameObject.GetComponent<DraggableModule>();

        stateController.gameObject.transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Check if any of this object's connectors are close to another connector
        foreach (ModuleConnection connector in module.GetConnectors())
        {
            GameObject nearestConnector = connector.GetNearestConnector();

            if (nearestConnector == null)
            {
                continue;
            }

            if (!nearestConnector.GetComponent<ModuleConnection>().isOccupied && !module.OverlapsOtherModules())
            {
                module.SnapToConnector(connector, nearestConnector.GetComponent<ModuleConnection>());
                stateController.ChangeState(stateController.SnappedState);

                break;
            }
        }
    }
}
