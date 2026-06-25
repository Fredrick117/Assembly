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

    [SerializeField]
    private ModuleConnection testChildConnector;
    [SerializeField]
    private ModuleConnection testOtherConnector;

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
        if (!ModuleManager.Instance.testModeOn)
        {
            stateController.ChangeState(stateController.PlacingState);
            initialPickupPosition = transform.position;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // o snap!
            SnapToConnector(testChildConnector, testOtherConnector);
        }
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
        // Rotate the module to match the opposite angle of the other connector
        float angleDifference = otherConnector.transform.eulerAngles.z - childConnector.transform.localEulerAngles.z + 180f;
        transform.rotation = Quaternion.Euler(0, 0, angleDifference);

        // Match the positions of both connectors
        Vector3 childWorldPos = childConnector.transform.position;
        Vector3 otherWorldPos = otherConnector.transform.position;
        Vector3 offset = otherWorldPos - childWorldPos;
        transform.position += offset;

        otherConnector.isOccupied = true;
        childConnector.isOccupied = true;
        childConnector.linkedConnector = otherConnector.gameObject;
        otherConnector.linkedConnector = childConnector.gameObject;

        spriteRenderer.color = validPlacementColor;
    }

    public void ChangePlacementColor(bool isValidPlacement)
    {
        spriteRenderer.color = isValidPlacement ? validPlacementColor : invalidPlacementColor;
    }
}
