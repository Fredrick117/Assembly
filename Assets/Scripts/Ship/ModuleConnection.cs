using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ConnectorType
{
    ENGINE,
    ROOM,
    WEAPON
}

public class ModuleConnection : MonoBehaviour
{
    public bool IsOccupied;
    public ConnectorType Type;
    public Sprite ConnectorSprite;
    public GameObject LinkedConnector = null;

    private void Awake()
    {
        gameObject.tag = "Connector";
    }
}
