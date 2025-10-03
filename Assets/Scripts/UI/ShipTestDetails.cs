using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShipTestDetails : MonoBehaviour
{
    [SerializeField]
    private TMP_Text speedText;

    public GameObject ship;
    private Rigidbody2D shipRb;

    private void Start()
    {
        shipRb = ship.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        speedText.text = $"currentSpeed: {Mathf.Round(shipRb.velocity.magnitude * 10000)} m/s";
    }
}
