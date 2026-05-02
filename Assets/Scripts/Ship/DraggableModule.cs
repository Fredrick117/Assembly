using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class DraggableModule : MonoBehaviour
{
    public bool IsDragging = false;
    private Vector3 Offset;

    private Transform[] ConnectorsArray;

    private Vector3 InitialPickupPosition;

    private bool isOver;

    private void Awake()
    {
        gameObject.tag = "ShipModule";
    }

    private void Start()
    {
        ConnectorsArray = gameObject.transform.GetComponentsInChildren<Transform>();
        ConnectorsArray = ConnectorsArray.Where(child => child.tag == "Connector").ToArray();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsDragging)
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
        Offset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        IsDragging = true;
        InitialPickupPosition = transform.position;

        for (int i = 0; i < ConnectorsArray.Length; i++)
        {
            ModuleConnection Connection = ConnectorsArray[i].gameObject.GetComponent<ModuleConnection>();
            if (Connection.LinkedConnector != null)
            {
                Connection.IsOccupied = false;
                Connection.LinkedConnector.GetComponent<ModuleConnection>().IsOccupied = false;
                Connection.LinkedConnector.GetComponent<ModuleConnection>().LinkedConnector = null;
                Connection.LinkedConnector = null;
            }
        }
    }

    private void OnMouseUp()
    {
        IsDragging = false;
        float ClosestConnectorDistance = 99999.0f;
        GameObject ClosestOtherConnector = null;
        GameObject ClosestChildConnector = null;

        // Check if any of this object's connectors are close to another object's connector
        for (int i = 0; i < ConnectorsArray.Length; i++)
        {
            Collider2D[] HitColliders = Physics2D.OverlapCircleAll(transform.position, 1.0f);

            if (HitColliders.Length == 0)
                continue;

            foreach (var Hit in HitColliders)
            {
                float Distance = Vector2.Distance(ConnectorsArray[i].transform.position, Hit.gameObject.transform.position);

                if (Hit.gameObject.tag == "Connector"
                && Distance < ClosestConnectorDistance
                && Hit.gameObject.transform.parent != gameObject.transform)
                {
                    ClosestConnectorDistance = Distance;
                    ClosestOtherConnector = Hit.gameObject;
                    ClosestChildConnector = ConnectorsArray[i].gameObject;
                }
            }
        }

        if (ClosestChildConnector != null && ClosestOtherConnector != null && !ClosestOtherConnector.GetComponent<ModuleConnection>().IsOccupied)
        {
            transform.position = ClosestOtherConnector.transform.position - ClosestChildConnector.transform.localPosition;
            ClosestOtherConnector.GetComponent<ModuleConnection>().IsOccupied = true;
            ClosestChildConnector.GetComponent<ModuleConnection>().IsOccupied = true;
            ClosestChildConnector.GetComponent<ModuleConnection>().LinkedConnector = ClosestOtherConnector;
            ClosestOtherConnector.GetComponent<ModuleConnection>().LinkedConnector = ClosestChildConnector;
        }
    }
}
