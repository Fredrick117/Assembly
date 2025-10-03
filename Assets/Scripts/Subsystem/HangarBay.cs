using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewHangarBay", menuName = "Subsystem/Hangar Bay")]
public class HangarBay : Subsystem
{
    public int maxStarfighters;

    public override void ApplyToShip(ShipStats ship)
    {
        ship.currentMaxStarfighters += maxStarfighters;
        base.ApplyToShip(ship);
    }

    public override void RemoveFromShip(ShipStats ship)
    {
        ship.currentMaxStarfighters -= maxStarfighters;
        base.RemoveFromShip(ship);
    }
}
