using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoreShipModule : MonoBehaviour
{
    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.W))
        {
            print("Accelerate!");
        }

        if (Input.GetKey(KeyCode.S))
        {
            print("Decelerate!");
        }
    }
}
