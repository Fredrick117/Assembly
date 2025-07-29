using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewHangarBay", menuName = "Subsystem/Hangar Bay")]
public class HangarBay : Subsystem
{
    public int maxStarfighters;

    public override void ApplyToShip(ShipStats ship)
    {
        base.ApplyToShip(ship);
        ship.Starfighters += maxStarfighters;
    }

    public override void RemoveFromShip(ShipStats ship)
    {
        base.RemoveFromShip(ship);
        ship.Starfighters -= maxStarfighters;
    }
}
