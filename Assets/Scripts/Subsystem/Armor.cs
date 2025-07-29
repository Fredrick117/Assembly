using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ArmorMaterial
{
    None,
    Cardboard,
    Steel,
    Diamond,
}

[CreateAssetMenu(fileName = "NewArmor", menuName = "Subsystem/Armor")]
public class Armor : Subsystem
{
    public int rating;
    public ArmorMaterial armorMaterial;

    public override void ApplyToShip(ShipStats ship)
    {
        base.ApplyToShip(ship);
        ship.Armor += rating;
        ship.ArmorMaterial = armorMaterial;
    }

    public override void RemoveFromShip(ShipStats ship)
    {
        base.RemoveFromShip(ship);
        ship.Armor -= rating;
        ship.ArmorMaterial = ArmorMaterial.None;
    }
}
