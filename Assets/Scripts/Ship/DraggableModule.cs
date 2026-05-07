using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class DraggableModule : MonoBehaviour
{
    public bool isDragging = false;
    private Vector3 offset;

    private ModuleConnection[] connectors;

    private Vector3 initialPickupPosition;

    private bool isOver;

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

    // Update is called once per frame
    private void Update()
    {
        if (isDragging)
        {
            transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
    }

    private void OnMouseOver()
    {
        //if (Input.GetMouseButtonDown(1))
        //    Destroy(gameObject);
    }

    private void OnMouseDown()
    {
        isDragging = true;
        offset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        initialPickupPosition = transform.position;

        // Clear all linked connectors
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

    private void OnMouseUp()
    {
        isDragging = false;
        bool foundValidConnector = false;

        foreach (ModuleConnection connector in connectors)
        {
            GameObject nearestConnector = connector.GetNearestConnector();

            if (nearestConnector == null/* || nearestConnector.GetComponent<ModuleConnection>().IsOccupied*/)
            {
                continue;
            }
            
            if (!nearestConnector.GetComponent<ModuleConnection>().IsOccupied)
            {
                transform.position = nearestConnector.transform.position - connector.transform.localPosition;
                nearestConnector.GetComponent<ModuleConnection>().IsOccupied = true;
                connector.IsOccupied = true;
                connector.LinkedConnector = nearestConnector;
                nearestConnector.GetComponent<ModuleConnection>().LinkedConnector = nearestConnector;

                foundValidConnector = true;
            }
        }

        


        // Check if any of this object's connectors are close to another object's connector
        //for (int i = 0; i < connectors.Length; i++)
        //{
        //    Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, 1.0f);

        //    if (hitColliders.Length == 0)
        //        continue;

        //    foreach (var hit in hitColliders)
        //    {
        //        float distance = Vector2.Distance(connectors[i].transform.position, hit.gameObject.transform.position);

        //        if (hit.gameObject.tag == "Connector"
        //        && distance < closestConnectorDistance
        //        && hit.gameObject.transform.parent != gameObject.transform)
        //        {
        //            closestConnectorDistance = distance;
        //            closestOtherConnector = hit.gameObject;
        //            closestChildConnector = connectors[i].gameObject;
        //        }
        //    }
        //}

        //if (closestChildConnector != null && closestOtherConnector != null && !closestOtherConnector.GetComponent<ModuleConnection>().IsOccupied)
        //{
        //    transform.position = closestOtherConnector.transform.position - closestChildConnector.transform.localPosition;
        //    closestOtherConnector.GetComponent<ModuleConnection>().IsOccupied = true;
        //    closestChildConnector.GetComponent<ModuleConnection>().IsOccupied = true;
        //    closestChildConnector.GetComponent<ModuleConnection>().LinkedConnector = closestOtherConnector;
        //    closestOtherConnector.GetComponent<ModuleConnection>().LinkedConnector = closestChildConnector;
        //}
    }
}
