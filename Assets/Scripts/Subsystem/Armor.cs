using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewArmor", menuName = "Subsystem/Armor")]
public class Armor : Subsystem
{
    public int armorRating;
    public float massMultiplier;

    public override void ApplyToShip(ShipStats ship)
    {
        ship.currentArmorRating += armorRating;
        ship.currentMass += Mathf.RoundToInt(ship.baseStats.baseMass * massMultiplier);

        base.ApplyToShip(ship);
    }

    public override void RemoveFromShip(ShipStats ship)
    {
        ship.currentArmorRating -= armorRating;
        ship.currentMass -= Mathf.RoundToInt(ship.baseStats.baseMass * massMultiplier);

        base.RemoveFromShip(ship);
    }
}
