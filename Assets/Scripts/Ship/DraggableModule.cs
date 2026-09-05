using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ModuleInput
{
    public Vector2 mousePosition;

    public bool isRightMouseDown;
    public bool isRightMouseUp;

    public bool isLeftMouseDown;
    public bool isLeftMouseUp;

    public bool isSpacebarPressed;

    public bool isMouseOver;
}

public class DraggableModule : MonoBehaviour
{
    public float mass = 1f;

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

    [HideInInspector]
    public Color originalColor;
    
    public static float mouseUnsnapDistance = 0.7f;

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

        originalColor = spriteRenderer.color;
    }

    private void Start()
    {
        this.stateController.ChangeState(stateController.PlacingState);
        initialPickupPosition = transform.position;
    }

    public ModuleConnection[] GetConnectors()
    {
        return connectors;
    }

    public void Ghostify()
    {
        Color translucent = originalColor;
        translucent.a = 0.5f;
        spriteRenderer.color = translucent;

        ModuleManager.Instance.shipCore.GetComponent<Rigidbody2D>().isKinematic = true;
    }

    public void UnGhostify()
    {
        spriteRenderer.color = originalColor;
        ModuleManager.Instance.shipCore.GetComponent<Rigidbody2D>().isKinematic = false;
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
        transform.parent = ModuleManager.Instance.shipCore.transform;
        ModuleManager.Instance.ghostModule = null;
        UnGhostify();
    }

    public void PickUpModule()
    {
        AttachToMouse();
        ClearConnectors();
        Ghostify();
    }

    private void OnDestroy()
    {
        CoreShipModule core = ModuleManager.Instance?.shipCore?.GetComponent<CoreShipModule>();

        if (core != null)
        {
            core.RecalculateMass();
        }
    }

    public void SnapToConnector(ModuleConnection childConnector, ModuleConnection otherConnector)
    {
        transform.parent = otherConnector.transform.parent;

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
