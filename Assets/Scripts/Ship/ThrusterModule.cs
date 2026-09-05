using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrusterModule : MonoBehaviour
{
    [HideInInspector]
    public Rigidbody2D shipRigidbody;

    public float thrustMagnitude = 500.0f;

    [HideInInspector]
    public bool isFiring = false;

    private void Awake()
    {
        shipRigidbody = ModuleManager.Instance.shipCore.GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (isFiring)
        {
            shipRigidbody.AddForce(transform.up * thrustMagnitude * Time.fixedDeltaTime);
        }
    }
}
