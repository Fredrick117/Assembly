using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour
{
    public bool isDragging = false;

    private Vector3 offset;

    private Vector3 initialPickupPosition = Vector3.negativeInfinity;

    private Rigidbody2D rb;

    private Color originalColor;

    private Color validPlacementColor = Color.green;

    private Color invalidPlacementColor = Color.red;

    [SerializeField]
    private SpriteRenderer hullSprite;

    private ShipModule shipModule;

    private bool canPlace = false;

    private bool isSnappedToConnector = false;

    private void Awake()
    {
        originalColor = hullSprite.color;
    }

    private void Start()
    {
        shipModule = GetComponent<ShipModule>();

        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (isDragging)
        {
            if (!isSnappedToConnector)
                MoveModuleToMousePosition();

            canPlace = GetCanPlaceModule();

            hullSprite.color = canPlace ? validPlacementColor : invalidPlacementColor;
        }

        // Fixes a bug with instantiated objects not able to fire OnMouseUp events...
        if (Input.GetMouseButtonUp(0))
        {
            if (this.isDragging)
                PlaceObject();
        }
    }

    private void OnMouseDown()
    {
        if (!this.isDragging)
            PickUpObject();
    }

    private void OnMouseUp()
    {
        if (this.isDragging)
            PlaceObject();
    }

    private void MoveModuleToMousePosition()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        rb.position = mousePosition;
    }

    private bool GetCanPlaceModule()
    {
        if (ShipManager.Instance.rootModule == null || ShipManager.Instance.rootModule == this.gameObject)
        {
            return true;
        }

        if (shipModule.IsColliding())
        {
            Debug.LogError("Cannot place, is colliding!");
            return false;
        }

        // Check if colliding with valid components
        foreach (Connector connector in shipModule.connectors)
        {
            Collider2D[] nearbyConnectors = Physics2D.OverlapCircleAll(connector.transform.position, 0.1f, LayerMask.GetMask("Connector"));

            foreach (Collider2D hit in nearbyConnectors)
            {
                Connector nearbyConnector = hit.GetComponent<Connector>();

                if (nearbyConnector == null || nearbyConnector == connector || nearbyConnector.transform.IsChildOf(transform))
                {
                    continue;
                }

                if (connector.type == nearbyConnector.type)
                {
                    if (!isSnappedToConnector)
                    {
                        float thisConnectorOffset = Vector2.Distance(gameObject.transform.position, connector.transform.position);

                        transform.position = nearbyConnector.transform.position + new Vector3(thisConnectorOffset, 0, 0);

                        isSnappedToConnector = true;
                    }

                    return true;
                }
            }
        }

        return false;
    }

    public void PickUpObject()
    {
        print("pick up object");
        offset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        isDragging = true;

        RemoveAllConnections();

        initialPickupPosition = transform.position;
    }

    public void PlaceObject()
    {
        print("place object");

        isDragging = false;

        if (!canPlace)
        {
            if (Vector3.Equals(initialPickupPosition, Vector3.negativeInfinity))
            {
                Destroy(gameObject);
                return;
            }

            transform.position = initialPickupPosition;
        }

        if (ShipManager.Instance.rootModule == null)
        {
            print("I am the root!");
            ShipManager.Instance.rootModule = gameObject;
        }

        PopulateConnections();

        // TODO: fix
        hullSprite.color = originalColor;
    }

    private void RemoveAllConnections()
    {
        foreach (Connector connector in shipModule.connectors)
        {
            connector.RemoveAllConnections();

            if (connector.otherConnector)
                connector.otherConnector.RemoveAllConnections();
        }
    }

    private void PopulateConnections()
    {
        foreach (Connector connector in shipModule.connectors)
        {
            Collider2D[] nearbyConnectors = Physics2D.OverlapCircleAll(connector.transform.position, 0.1f, LayerMask.GetMask("Connector"));

            foreach (Collider2D hit in nearbyConnectors)
            {
                Connector nearbyConnector = hit.GetComponent<Connector>();

                if (nearbyConnector == null || nearbyConnector == connector || nearbyConnector.transform.IsChildOf(transform))
                {
                    continue;
                }

                if (connector.type == nearbyConnector.type)
                {
                    connector.otherConnector = nearbyConnector;
                    nearbyConnector.otherConnector = connector;
                }
            }
        }
    }
}
