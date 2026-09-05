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

    [HideInInspector]
    public bool isOccupied;

    [HideInInspector]
    public GameObject linkedConnector = null;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        gameObject.tag = "Connector";
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetVisible(bool visible)
    {
        spriteRenderer.enabled = visible;
    }

    public static void ShowAll()
    {
        foreach (GameObject connector in GameObject.FindGameObjectsWithTag("Connector"))
        {
            connector.GetComponent<ModuleConnection>().SetVisible(true);
        }
    }

    public static void HideAll()
    {
        foreach (GameObject connector in GameObject.FindGameObjectsWithTag("Connector"))
        {
            connector.GetComponent<ModuleConnection>().SetVisible(false);
        }
    }

    public GameObject GetNearestConnector()
    {
        GameObject nearestConnector = null;
        float nearestConnectorDistance = float.MaxValue;

        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, snapDistance);

        foreach (Collider2D hit in hitColliders)
        {
            if (hit.transform.parent == this.transform.parent || hit.gameObject.tag != "Connector" || hit.gameObject == gameObject)
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

        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(transform.position, transform.position + (-transform.up * connectorDirectionLength));
    }
}
