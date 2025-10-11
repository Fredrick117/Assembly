using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewHangarBay", menuName = "Subsystem/Hangar Bay")]
public class HangarBay : Subsystem
{
    public int maxCraft;

    public override void ApplyToShip(ShipStats ship)
    {
        ship.currentMaxCraft += maxCraft;
        base.ApplyToShip(ship);
    }

    public override void RemoveFromShip(ShipStats ship)
    {
        ship.currentMaxCraft -= maxCraft;
        base.RemoveFromShip(ship);
    }
}
