using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum ArmorRating
{
    None,
    D,
    C,
    B,
    A,
    S
}

[CreateAssetMenu(fileName = "NewArmor", menuName = "Subsystem/Armor")]
public class Armor : Subsystem
{
    public int armorRating;
    public float massMultiplier;
    public bool canEnterAtmosphere;

    public override void ApplyToShip(CurrentShipStats ship)
    {
        base.ApplyToShip(ship);

        if (ship.currentArmorRating <= armorRating)
            ship.currentArmorRating = armorRating;

        ship.canEnterAtmosphere = canEnterAtmosphere;
        ship.currentMass += Mathf.RoundToInt(ship.baseStats.baseMass * massMultiplier);
    }

    public override void RemoveFromShip(CurrentShipStats ship)
    {
        base.RemoveFromShip(ship);

        ship.currentArmorRating = ship.GetHighestArmorRating();
        ship.canEnterAtmosphere = false;
        ship.currentMass -= Mathf.RoundToInt(ship.baseStats.baseMass * massMultiplier);
    }
}
