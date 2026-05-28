using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DraggableModule : MonoBehaviour
{
    [HideInInspector]
    public bool isDragging = false;

    [HideInInspector]
    // The first module that is placed
    // TODO: all modules should be connected to this one
    public bool isRoot = false;

    [SerializeField]
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private Color validPlacementColor = new Color(0, 1, 0, 0.5f);
    [SerializeField]
    private Color invalidPlacementColor = new Color(1, 0, 0, 0.5f);
    
    public float mouseUnsnapDistance = 0.7f;

    private Vector3 offset;
    private ModuleConnection[] connectors;
    private Vector3 initialPickupPosition;
    private ModuleStateController stateController;  

    private void Awake()
    {
        if (GameObject.FindGameObjectsWithTag("ShipModule").Length == 0)
        {
            isRoot = true;
        }

        gameObject.tag = "ShipModule";

        stateController = gameObject.GetComponent<ModuleStateController>();
        connectors = gameObject.transform.GetComponentsInChildren<ModuleConnection>();
    }

    private void Start()
    {
        stateController.ChangeState(stateController.PlacingState);

        initialPickupPosition = transform.position;
    }

    public ModuleConnection[] GetConnectors()
    {
        return connectors;
    }

    public void Ghostify()
    {
        Color originalColor = spriteRenderer.color;
        spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0.5f);
    }

    public void UnGhostify()
    {
        spriteRenderer.color = Color.white;
    }

    public void ClearConnectors()
    {
        foreach (ModuleConnection connector in connectors)
        {
            if (connector.linkedConnector != null)
            {
                connector.isOccupied = false;
                connector.linkedConnector.GetComponent<ModuleConnection>().isOccupied = false;
                connector.linkedConnector.GetComponent<ModuleConnection>().linkedConnector = null;
                connector.linkedConnector = null;
            }
        }
    }

    public bool OverlapsOtherModules()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, GetComponent<Collider2D>().bounds.size - new Vector3(0.1f, 0.1f, 0.1f), 0f);
        return colliders.Any(collider => collider.gameObject != gameObject && collider.CompareTag("ShipModule"));
    }

    public void AttachToMouse()
    {
        isDragging = true;
        offset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    public void PlaceModule()
    {
        ModuleManager.Instance.ghostModule = null;
        UnGhostify();
    }

    public void PickUpModule()
    {
        AttachToMouse();
        ClearConnectors();
        Ghostify();
    }

    public void SnapToConnector(ModuleConnection childConnector, ModuleConnection otherConnector)
    {
        transform.position = otherConnector.transform.position - childConnector.transform.localPosition;
        otherConnector.GetComponent<ModuleConnection>().isOccupied = true;
        childConnector.isOccupied = true;
        childConnector.linkedConnector = otherConnector.gameObject;
        otherConnector.GetComponent<ModuleConnection>().linkedConnector = otherConnector.gameObject;

        spriteRenderer.color = validPlacementColor;
    }

    public void ChangePlacementColor(bool isValidPlacement)
    {
        spriteRenderer.color = isValidPlacement ? validPlacementColor : invalidPlacementColor;
    }
}
