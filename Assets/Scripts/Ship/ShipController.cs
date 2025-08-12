using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    private Rigidbody2D rb;

    public float maxSpeed = 10f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.W))
        {
            rb.AddForce(new Vector2(0.0f, 1f));
        }

        if (Input.GetKey(KeyCode.S))
        {
            rb.AddForce(new Vector2(0.0f, -1f));
        }

        if (Input.GetKey(KeyCode.A))
        {
            rb.AddForce(new Vector2(-1f, 0.0f));
        }

        if (Input.GetKey(KeyCode.D))
        {
            rb.AddForce(new Vector2(1f, 0.0f));
        }

        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
    }
}
