using UnityEngine;

public class PlacingState : IState
{
    public float currentRotationDifference = 0.0f;

    public void HandleInput(ModuleStateController stateController, ModuleInput input)
    {
        if (input.isSpacebarPressed)
        {
            currentRotationDifference += 90.0f;
            // Rotate this object 90 degrees(?)
            // Should it be 90 degrees or should it just try to match the rotation of the "next" connector?
            stateController.transform.rotation = Quaternion.Euler(0, 0, stateController.transform.position.z - currentRotationDifference);
            Debug.Log(stateController.transform.rotation);
        }

        if (!input.isMouseOver)
        {
            return;
        }

        if (stateController.gameObject.GetComponent<DraggableModule>().isRoot && input.isLeftMouseDown)
        {
            stateController.ChangeState(stateController.ConnectedState);
        }

        if (input.isMouseOver && input.isRightMouseDown)
        {
            ModuleConnection.HideAll();
            GameObject.Destroy(stateController.gameObject);
            return;
        }
    }

    public void OnEnter(ModuleStateController stateController, IState previousState)
    {
        //Debug.Log($"({stateController.gameObject.name}) [PlacingState] OnEnter!");
        DraggableModule module = stateController.gameObject.GetComponent<DraggableModule>();
        module.PickUpModule();
        module.ChangePlacementColor(module.isRoot);

        // Show all connectors
        ModuleConnection.ShowAll();
    }

    public void OnExit(ModuleStateController stateController)
    {
        //Debug.Log("[PlacingState] OnExit!");
    }

    public void UpdateState(ModuleStateController stateController)
    {
        DraggableModule module = stateController.gameObject.GetComponent<DraggableModule>();

        stateController.transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);

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
