using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShipModule : Draggable
{
    [HideInInspector]
    public Connector[] connectors;

    public int subsystemSlots = 0;
    public int mass = 0;
    
    [SerializeField]
    private float maxSnapDistance = 2.0f;
    [SerializeField]
    private SpriteRenderer hullSprite;
    private bool canPlace = false;
    private Vector3 initialPickupPosition = Vector3.negativeInfinity;

    private Grid grid;

    [SerializeField] private SpriteRenderer placementStatusSprite;

    private void Awake()
    {
        originalColor = Color.white;

        grid = GameObject.Find("Grid").GetComponent<Grid>();
    }

    protected override void Start()
    {
        base.Start();
        
        connectors = gameObject.GetComponentsInChildren<Connector>();
    }

    private void OnEnable()
    {
        EventManager.OnClear += DestroyModule;
    }

    private void OnDisable()
    {
        EventManager.OnClear -= DestroyModule;
    }

    private void Update()
    {
        if (!isDragging)
        {
            return;
        }

        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int cellPosition = grid.WorldToCell(mousePosition);
        transform.position = grid.GetCellCenterWorld(cellPosition);

        canPlace = GetCanPlaceModule();

        placementStatusSprite.color = canPlace ? validPlacementColor : invalidPlacementColor;
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Destroy(gameObject);
        }
    }
    
    private void OnMouseDown()
    {
        if (!this.isDragging)
        {
            this.isDragging = true;
            PickUpObject();
        }
        else
        {
            this.isDragging = false;
            PlaceObject();
        }
    }
    
    private bool GetCanPlaceModule()
    {
        if (ShipManager.Instance.rootModule == null || ShipManager.Instance.rootModule == this.gameObject)
        {
            return true;
        }

        return !IsColliding();
    }

    private bool IsColliding()
    {
        Collider2D[] collisions = Physics2D.OverlapBoxAll(transform.position, new Vector2(0.9f, 0.9f), 0f);

        if (collisions.Length > 0)
        {
            if (collisions.Any(x => x.gameObject.CompareTag("ShipModule") && x.gameObject != this.gameObject))
            {
                return true;
            }
        }
        
        return false;
    }
    
    private void PopulateConnections()
    {
        foreach (Connector connector in connectors)
        {
            Collider2D[] nearbyConnectors = Physics2D.OverlapCircleAll(connector.transform.position, 0.1f, LayerMask.GetMask("Connector"));

            foreach (Collider2D hit in nearbyConnectors)
            {
                Connector nearbyConnector = hit.GetComponent<Connector>();

                if (nearbyConnector == null || nearbyConnector == connector || nearbyConnector.transform.IsChildOf(transform))
                {
                    Debug.LogWarning("No valid nearby connectors found.");
                    continue;
                }

                if (connector.type != nearbyConnector.type) continue;
                
                connector.otherConnector = nearbyConnector;
                nearbyConnector.otherConnector = connector;
                
                Debug.Log("Connected connector to other connector");
            }
        }
    }
    
    private void PickUpObject()
    {
        isDragging = true;

        RemoveAllConnections();

        initialPickupPosition = transform.position;

        placementStatusSprite.gameObject.SetActive(true);
    }

    private void PlaceObject()
    {
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

        placementStatusSprite.gameObject.SetActive(false);
    }

    private void SetPlacementStatusColor()
    {
        placementStatusSprite.color = canPlace ? validPlacementColor : invalidPlacementColor;
    }

    private void RemoveAllConnections()
    {
        foreach (Connector connector in connectors)
        {
            connector.RemoveAllConnections();

            if (connector.otherConnector)
                connector.otherConnector.RemoveAllConnections();
        }
    }

    private void DestroyModule()
    {
        GameObject.Destroy(gameObject);
    }
}
