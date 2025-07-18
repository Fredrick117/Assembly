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
        base.ApplyToShip(ship);
        ship.ShieldStrength += shieldStrength;
    }

    public override void RemoveFromShip(ShipStats ship)
    {
        base.RemoveFromShip(ship);
        ship.ShieldStrength -= shieldStrength;
    }
}
