using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewHangarBay", menuName = "Subsystem/Hangar Bay")]
public class HangarBay : Subsystem
{
    public int maxCraft;

    public override void ApplyToShip(CurrentShipStats ship)
    {
        ship.currentMaxCraft += maxCraft;
        base.ApplyToShip(ship);
    }

    public override void RemoveFromShip(CurrentShipStats ship)
    {
        ship.currentMaxCraft -= maxCraft;
        base.RemoveFromShip(ship);
    }
}
