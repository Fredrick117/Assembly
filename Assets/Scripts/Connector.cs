using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public enum ConnectorSize
{
    Large,
    Medium,
    Small,
    Tiny,
}

[Serializable]
public enum ConnectorDirection
{
    North,
    South,
    East,
    West,
}

[Serializable]
public enum ArmorType
{
    Iron,
    Steel,
    Titanium
}

public class Connector : MonoBehaviour
{
    public ConnectorSize type;
    
    public ConnectorDirection direction;

    [HideInInspector]
    public Connector otherConnector = null;

    [HideInInspector]
    public Color connectorColor = Color.black;

    private SpriteRenderer spriteRenderer;
    
    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        switch (type)
        {
            case ConnectorSize.Tiny:
                spriteRenderer.color = Color.yellow;
                break;
            
            case ConnectorSize.Small:
                spriteRenderer.color = Color.green;
                break;

            case ConnectorSize.Medium:
                spriteRenderer.color = Color.cyan;
                break;

            case ConnectorSize.Large:
                spriteRenderer.color = Color.red;
                break;
        }
    }

    public void RemoveAllConnections()
    {
        otherConnector = null;
    }

    public List<GameObject> GetNearbyValidConnectors()
    {
        List<GameObject> validConnectors = new();
        Collider2D[] nearbyConnectors = Physics2D.OverlapCircleAll(transform.position, 0.1f, LayerMask.GetMask("Connector"));

        foreach (Collider2D hit in nearbyConnectors)
        {
            Connector nearbyConnector = hit.GetComponent<Connector>();

            if (nearbyConnector == null)
                continue;

            if (nearbyConnector == this)
                continue;
            
            if (nearbyConnector.transform.IsChildOf(transform)) 
                continue;

            if (this.direction != Utilities.GetOppositeDirection(nearbyConnector.direction))
                continue;

            validConnectors.Add(nearbyConnector.gameObject);
        }

        return validConnectors;
    }
}
