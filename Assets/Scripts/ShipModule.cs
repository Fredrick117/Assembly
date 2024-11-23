using System.Collections.Generic;
using UnityEngine;

public enum ModuleType
{
    Hull,
    Hangar,
    Engine,
    Cockpit
}

public class ShipModule : Draggable
{
    public ModuleType moduleType;

    [HideInInspector]
    public bool isConnectedToRoot = false;

    [HideInInspector]
    public bool isCollidingWithShipModule = false;

    [HideInInspector]
    public List<ShipModule> connectedModules = new List<ShipModule>();

    [HideInInspector]
    public Connector[] connectors;

    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        connectors = gameObject.GetComponentsInChildren<Connector>();
        print("Num of connectors: " + connectors.Length);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Destroy(gameObject);
        }
    }

    public bool IsColliding()
    {
        Collider2D[] collisions = Physics2D.OverlapBoxAll(transform.position, new Vector2(0.85f, 0.85f), 0f);

        if (collisions.Length > 0)
        {
            foreach (Collider2D col in collisions)
            {
                if (col.gameObject.CompareTag("ShipModule") && !Utilities.IsChild(gameObject, col.gameObject))
                {
                    print(col.gameObject.name);
                    return true;
                }
            }
        }

        return false;
    }
}
