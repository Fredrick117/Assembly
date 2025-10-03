using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewShielding", menuName = "Subsystem/Shielding")]
public class Shielding : Subsystem
{
    public int shieldStrength;
    public int rechargeSpeed;

    public override void ApplyToShip(ShipStats ship)
    {
        ship.currentShielding += shieldStrength;
        base.ApplyToShip(ship);
    }

    public override void RemoveFromShip(ShipStats ship)
    {
        ship.currentShielding -= shieldStrength;
        base.RemoveFromShip(ship);
    }
}
