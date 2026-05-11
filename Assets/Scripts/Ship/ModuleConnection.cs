using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ConnectorType
{
    Engine,
    Weapon,
}

public class ModuleConnection : MonoBehaviour
{
    [SerializeField]
    private float connectorDirectionLength = 0.5f;

    [SerializeField]
    private float snapDistance = 0.3f;

    public Vector2 direction = Vector2.right;
    public bool isOccupied;
    public GameObject linkedConnector = null;

    private void Awake()
    {
        gameObject.tag = "Connector";
    }

    public GameObject GetNearestConnector()
    {
        GameObject nearestConnector = null;
        float nearestConnectorDistance = float.MaxValue;

        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, snapDistance);

        foreach (Collider2D hit in hitColliders)
        {
            if (hit.transform.parent == this.transform.parent || hit.gameObject.tag != "Connector")
            {
                continue;
            }

            float distance = Vector2.Distance(transform.position, hit.transform.position);

            if (distance < nearestConnectorDistance)
            {
                nearestConnector = hit.gameObject;
            }
        }

        return nearestConnector;
    }

    void OnDrawGizmos()
    {
        if (GetNearestConnector() != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, 0.5f);
        }

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + (Vector3)direction * connectorDirectionLength);
    }
}
