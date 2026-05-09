using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DraggableModule : MonoBehaviour
{
    [HideInInspector]
    public bool isDragging = false;

    [SerializeField]
    private SpriteRenderer spriteRenderer;
    
    [SerializeField]
    private float mouseUnsnapDistance = 0.7f;

    private Vector3 offset;
    private ModuleConnection[] connectors;
    private Vector3 initialPickupPosition;
    private bool isSnapped = false;

    private void Awake()
    {
        gameObject.tag = "ShipModule";
    }

    private void Start()
    {
        connectors = gameObject.transform.GetComponentsInChildren<ModuleConnection>();
        connectors = connectors.Where(child => child.CompareTag("Connector")).ToArray();
        
        initialPickupPosition = transform.position;
    }

    private void Update()
    {
        if (!isDragging)
        {
            return;
        }

        if (isSnapped)
        {
            Vector2 mousePosition = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
            float distance = Vector2.Distance(transform.position, mousePosition);

            if (distance > mouseUnsnapDistance)
            {
                isSnapped = false;
                ClearConnectors();
            }

            return;
        }
        
        transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Check if any of this object's connectors are close to another connector
        foreach (ModuleConnection connector in connectors)
        {
            GameObject nearestConnector = connector.GetNearestConnector();

            if (nearestConnector == null)
            {
                continue;
            }

            if (!nearestConnector.GetComponent<ModuleConnection>().IsOccupied && !OverlapsOtherModules())
            {
                SnapToConnector(connector, nearestConnector.GetComponent<ModuleConnection>());
                break;
            }
        }
    }

    private void OnMouseDown()
    {
        if (isDragging)
        {
            PlaceModule();
            UnGhostify();
        }
        else
        {
            PickUpModule();
            Ghostify();
        }
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

    private void ClearConnectors()
    {
        foreach (ModuleConnection connector in connectors)
        {
            if (connector.LinkedConnector != null)
            {
                connector.IsOccupied = false;
                connector.LinkedConnector.GetComponent<ModuleConnection>().IsOccupied = false;
                connector.LinkedConnector.GetComponent<ModuleConnection>().LinkedConnector = null;
                connector.LinkedConnector = null;
            }
        }
    }

    private void PlaceModule()
    {
        RemoveFromMouse();
        spriteRenderer.color = Color.white;
        ModuleManager.Instance.ghostModule = null;
    }

    private void PickUpModule()
    {
        AttachToMouse();
        ClearConnectors();
    }

    private bool OverlapsOtherModules()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, GetComponent<Collider2D>().bounds.size - new Vector3(0.1f, 0.1f, 0.1f), 0f);
        return colliders.Any(collider => collider.gameObject != gameObject && collider.CompareTag("ShipModule"));
    }

    private void AttachToMouse()
    {
        isDragging = true;
        offset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void RemoveFromMouse()
    {
        isDragging = false;
    }

    private void SnapToConnector(ModuleConnection childConnector, ModuleConnection otherConnector)
    {
        isSnapped = true;
        transform.position = otherConnector.transform.position - childConnector.transform.localPosition;
        otherConnector.GetComponent<ModuleConnection>().IsOccupied = true;
        childConnector.IsOccupied = true;
        childConnector.LinkedConnector = otherConnector.gameObject;
        otherConnector.GetComponent<ModuleConnection>().LinkedConnector = otherConnector.gameObject;
    }
}
