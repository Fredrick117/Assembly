using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor.MemoryProfiler;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class DraggableModule : MonoBehaviour
{
    [HideInInspector]
    public bool isDragging = false;

    private Vector3 offset;
    private ModuleConnection[] connectors;
    private Vector3 initialPickupPosition;
    private bool isSnapped = false;

    [SerializeField]
    private float mouseUnsnapDistance = 0.7f;

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
            //print(distance);

            if (distance > mouseUnsnapDistance)
            {
                isSnapped = false;
                ClearConnectors();
                print("unsnap!");
            }

            return;
        }
        
        transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Check if any of this object's connectors are close to another object's connector
        foreach (ModuleConnection connector in connectors)
        {
            GameObject nearestConnector = connector.GetNearestConnector();

            if (nearestConnector == null)
            {
                continue;
            }

            if (!nearestConnector.GetComponent<ModuleConnection>().IsOccupied)
            {
                // Snap to the nearest connector
                print("snap!");
                SnapToConnector(connector, nearestConnector.GetComponent<ModuleConnection>());
                break; // Exit the loop after snapping to one connector
            }
        }

        // If there is a valid connector, snap to it

        // If there is not a valid connector, move to mouse position
    }

    private void OnMouseDown()
    {
        if (isDragging)
        {
            RemoveFromMouse();
            ClearConnectors();
        }
        else
        {
            AttachToMouse();
        }
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
