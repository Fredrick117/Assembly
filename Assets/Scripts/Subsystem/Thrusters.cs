using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewThrusters", menuName = "Subsystem/Thrusters")]
public class Thrusters : Subsystem
{
    public int maxSpeed;
    public bool atmosphericEntryCapable;
}
