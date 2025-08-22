using System.Collections.Generic;
using UnityEngine;

public class ShipModule : MonoBehaviour
{
    [HideInInspector]
    public Connector[] connectors;

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
