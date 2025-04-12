using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSubsystemStats", menuName = "Ship/Subsystem")]
public class SubsystemStats : ScriptableObject
{
    public string displayName;
    public int powerOutput;
    public int powerDraw;
}
