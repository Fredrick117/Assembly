using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public enum ConnectorType
{
    Module, Thruster, Weapon
};

public class Connector : MonoBehaviour
{
    public ConnectorType type;

    //public GameObject connectedObject = null;
    public Connector otherConnector = null;

    public Color connectorColor = Color.black;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        switch (type)
        {
            case ConnectorType.Module:
                spriteRenderer.color = Color.grey;
                break;

            case ConnectorType.Thruster:
                spriteRenderer.color = Color.cyan;
                break;

            case ConnectorType.Weapon:
                spriteRenderer.color = Color.red;
                break;

            default:
                break;
        }
    }
}
