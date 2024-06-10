using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Connectable : MonoBehaviour
{
    private Transform[] connectorsArray;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        connectorsArray = gameObject.transform.GetComponentsInChildren<Transform>();
        connectorsArray = connectorsArray.Where(child => child.tag == "Connector").ToArray();
    }

    private void OnMouseDown() 
    {
        // Sever all connections
        // for (int i = 0; i < connectorsArray.Length; i++)
        // {
        //     ModuleConnection connection = connectorsArray[i].gameObject.GetComponent<ModuleConnection>();
        //     if (connection.LinkedConnector != null)
        //     {
        //         connection.IsOccupied = false;
        //         connection.LinkedConnector.GetComponent<ModuleConnection>().IsOccupied = false;
        //         connection.LinkedConnector.GetComponent<ModuleConnection>().LinkedConnector = null;
        //         connection.LinkedConnector = null;
        //     }
        // }
    }

    private void OnMouseUp() 
    {
        float closestConnectorDistance = 99999.0f;
        GameObject closestOtherConnector = null;
        GameObject closestChildConnector = null;

        // Check if any of this object's connectors are close to another object's connector
        for (int i = 0; i < connectorsArray.Length; i++)
        {
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, 1.0f);

            if (hitColliders.Length == 0)
                continue;

            foreach (Collider2D hit in hitColliders)
            {
                Vector2 connectorDirection = hit.gameObject.transform.position - gameObject.transform.position;
                Rigidbody2D otherRB = hit.gameObject.GetComponent<Rigidbody2D>();
                rb.velocity = connectorDirection * 2f;
            }

            foreach (var hit in hitColliders)
            {
                float connectorDistance = Vector2.Distance(connectorsArray[i].transform.position, hit.gameObject.transform.position);

                if (hit.gameObject.CompareTag("Connector")
                && connectorDistance < closestConnectorDistance
                && hit.gameObject.transform.parent != gameObject.transform)
                {
                    closestConnectorDistance = connectorDistance;
                    closestOtherConnector = hit.gameObject;
                    closestChildConnector = connectorsArray[i].gameObject;
                }
            }
        }

        if (closestChildConnector != null && closestOtherConnector != null && !closestOtherConnector.GetComponent<ModuleConnection>().isOccupied)
        {
            transform.position = closestOtherConnector.transform.position - closestChildConnector.transform.localPosition;
            closestOtherConnector.GetComponent<ModuleConnection>().isOccupied = true;
            closestChildConnector.GetComponent<ModuleConnection>().isOccupied = true;
            closestChildConnector.GetComponent<ModuleConnection>().linkedConnector = closestOtherConnector;
            closestChildConnector.GetComponent<ModuleConnection>().linkedConnector = closestChildConnector;
        }
    }
}
