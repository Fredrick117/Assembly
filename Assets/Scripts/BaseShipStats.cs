using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewShipStats", menuName = "Ship/Stats")]
public class BaseShipStats : ScriptableObject
{
    public Sprite sprite;
    public string shipClass;
    public int baseArmor;
    public int baseHull;
    public float baseSpeed;
    public int basePower;
}
