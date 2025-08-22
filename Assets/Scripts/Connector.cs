using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public enum ConnectorType
{
    Small,
};

public class Connector : MonoBehaviour
{
    public ConnectorType type;

    //public GameObject connectedObject = null;
    public Connector otherConnector = null;

    public Color connectorColor = Color.black;

    private SpriteRenderer spriteRenderer;

    public bool left;

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        switch (type)
        {
            case ConnectorType.Small:
                spriteRenderer.color = Color.cyan;
                break;

            //case ConnectorType.Medium:
            //    spriteRenderer.color = Color.cyan;
            //    break;

            //case ConnectorType.Large:
            //    spriteRenderer.color = Color.red;
            //    break;

            //case ConnectorType.Massive:
            //    spriteRenderer.color = Color.black;
            //    break;

            default:
                break;
        }
    }

    public void RemoveAllConnections()
    {
        otherConnector = null;
    }
}
