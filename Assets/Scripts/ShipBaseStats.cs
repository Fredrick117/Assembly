using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewShipStats", menuName = "Ship/Stats")]
public class ShipBaseStats : ScriptableObject
{
    public Sprite sprite;
    public string shipClass;
    public int baseArmor;
    public int baseHull;
    public float baseSpeed;
    public int basePower;
    public int baseMass;
    //public int basePrice;
    public int subsystemSlots;
}
