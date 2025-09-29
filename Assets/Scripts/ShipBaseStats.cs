using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewShipStats", menuName = "Ship/Stats")]
public class ShipBaseStats : ScriptableObject
{
    public Sprite baseSprite;
    public ShipClassification shipClass;
    //public int baseArmor;
    //public int baseHull;
    //public int baseSpeed;
    //public int basePower;
    public int baseMass;
    public int basePrice;
    public int weaponSlots;
    public int utilitySlots;
    public int reactorSlots;
}
