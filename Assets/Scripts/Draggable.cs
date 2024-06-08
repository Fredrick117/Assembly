using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Draggable : MonoBehaviour
{
    [HideInInspector]
    public bool isDragging = false;
    private Transform[] connectors;

    private Rigidbody2D rb;

    Vector3 offset;
    Vector3 dragStart;

    private void Start()
    {
        connectors = gameObject.transform.GetComponentsInChildren<Transform>();
        connectors = connectors.Where(child => child.tag == "Connector").ToArray();

        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (isDragging)
        {
            transform.position = new Vector3(mousePosition.x + offset.x, mousePosition.y + offset.y, transform.position.z);
            //transform.position = new Vector3((float)Math.Round(mousePosition.x), (float)Math.Round(mousePosition.y), transform.position.z);
        }
    }

    private void OnMouseDown()
    {
        print("mouse down");
        if (isDragging)
        {
            DropObject();
        }
        else
        {
            isDragging = true;
            offset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
            dragStart = transform.position;

            for (int i = 0; i < connectors.Length; i++)
            {
                ModuleConnection connection = connectors[i].gameObject.GetComponent<ModuleConnection>();
                if (connection.linkedConnector != null)
                {
                    connection.isOccupied = false;
                    connection.linkedConnector.GetComponent<ModuleConnection>().isOccupied = false;
                    connection.linkedConnector.GetComponent<ModuleConnection>().linkedConnector = null;
                    connection.linkedConnector = null;
                }
            }
        }
    }

    private void OnMouseUp()
    {
        print("mouse up");
        DropObject();
    }

    private void DropObject()
    {
        //isDragging = false;
        //GameManager.Instance.draggedObject = null;

        float closestConnectorDistance = 99999.0f;
        GameObject closestOtherConnector = null;
        GameObject closestChildConnector = null;

        // Check if any of this object's connectors are close to another object's connector
        for (int i = 0; i < connectors.Length; i++)
        {
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, 1.0f);

            if (hitColliders.Length == 0)
                continue;

            foreach (var hit in hitColliders)
            {
                float connectorDistance = Vector2.Distance(connectors[i].transform.position, hit.gameObject.transform.position);

                if (hit.gameObject.CompareTag("Connector")
                && connectorDistance < closestConnectorDistance
                && hit.gameObject.transform.parent != gameObject.transform)
                {
                    closestConnectorDistance = connectorDistance;
                    closestOtherConnector = hit.gameObject;
                    closestChildConnector = connectors[i].gameObject;
                }
            }
        }

        if (closestChildConnector != null && closestOtherConnector != null && !closestOtherConnector.GetComponent<ModuleConnection>().isOccupied)
        {
            transform.position = closestOtherConnector.transform.position - closestChildConnector.transform.localPosition;

            print("Child connector: " + closestChildConnector);
            print("Other connector: " + closestOtherConnector);
            print("transform.position: " + transform.position);
            print("closestChildConnector: " + closestChildConnector.transform.position);
            print("closestOtherConnector: " + closestOtherConnector.transform.position);

            closestOtherConnector.GetComponent<ModuleConnection>().isOccupied = true;
            closestChildConnector.GetComponent<ModuleConnection>().isOccupied = true;
            closestChildConnector.GetComponent<ModuleConnection>().linkedConnector = closestOtherConnector;
            closestChildConnector.GetComponent<ModuleConnection>().linkedConnector = closestChildConnector;
        }
    }
}
