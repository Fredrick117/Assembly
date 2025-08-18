using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    private Rigidbody2D rb;

    public float thrustForce = 10f;
    public float brakingForce = 8.0f;
    public float rotationSpeed = 100f;

    private float thrustInput;
    private float rotationInput;

    private bool isBraking = false;

    private float maxSpeed;

    public Ship shipStats;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        rb.mass = shipStats.mass;
        //maxSpeed = shipStats.speed;
        thrustForce = shipStats.speed;
    }

    private void Update()
    {
        thrustInput = Input.GetAxisRaw("Vertical");
        rotationInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetKey(KeyCode.Space) && !isBraking)
        {
            isBraking = true;
        }
        else if (isBraking)
        {
            isBraking = false;
        }
    }

    void FixedUpdate()
    {
        if (!isBraking)
        {
            if (rb.velocity.magnitude >= maxSpeed)
            {
                rb.AddForce(transform.up * thrustInput * thrustForce);
            }
            // TODO fix this shit
            rb.AddTorque(rotationSpeed * -rotationInput, ForceMode2D.Force);
        }
        else
        {
            rb.AddForce(-(rb.velocity * brakingForce));
        }
    }
}
