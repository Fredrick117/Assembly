using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoreShipModule : MonoBehaviour
{
    [SerializeField] private float coreMass = 1f;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.mass = coreMass;
    }

    public void RecalculateMass()
    {
        float total = coreMass;
        foreach (DraggableModule module in GetComponentsInChildren<DraggableModule>())
        {
            total += module.mass;
        }

        rb.mass = total;
    }

    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            ThrusterModule[] thrusters = GetComponentsInChildren<ThrusterModule>();

            foreach (ThrusterModule thruster in thrusters)
            {
                if (!thruster.isFiring)
                    thruster.isFiring = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            ThrusterModule[] thrusters = GetComponentsInChildren<ThrusterModule>();

            foreach (ThrusterModule thruster in thrusters)
            {
                if (thruster.isFiring)
                    thruster.isFiring = false;
            }
        }
    }
}
