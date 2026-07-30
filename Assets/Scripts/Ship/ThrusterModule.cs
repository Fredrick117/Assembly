using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrusterModule : MonoBehaviour
{
    [HideInInspector]
    public Rigidbody2D shipRigidbody;

    [HideInInspector]
    public bool isFiring = false;

    private void FixedUpdate()
    {
        Vector2 thrustDirection = transform.up;


    }
}
