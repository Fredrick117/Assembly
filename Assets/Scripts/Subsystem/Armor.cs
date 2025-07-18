using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ArmorMaterial
{
    Cardboard,
    Steel,
    Diamond,
}

[CreateAssetMenu(fileName = "NewArmor", menuName = "Subsystem/Armor")]
public class Armor : Subsystem
{
    public int rating;
    public float massIncrease;
    public ArmorMaterial armorMaterial;

    public override void ApplyToShip(ShipStats ship)
    {
        base.ApplyToShip(ship);
        ship.Mass = Mathf.RoundToInt(ship.Mass * massIncrease);
    }

    public override void RemoveFromShip(ShipStats ship)
    {
        base.RemoveFromShip(ship);

        if (massIncrease != 0)
            ship.Mass = Mathf.RoundToInt(ship.Mass / massIncrease); // If changes to mass are made before removal, is the final value correct?
    }
}
