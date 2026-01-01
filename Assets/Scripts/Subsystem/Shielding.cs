using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewShielding", menuName = "Subsystem/Shielding")]
public class Shielding : Subsystem
{
    public int shieldStrength;
    //public int rechargeSpeed;

    public override void ApplyToShip(CurrentShipStats ship)
    {
        ship.currentShielding += shieldStrength;
        base.ApplyToShip(ship);
    }

    public override void RemoveFromShip(CurrentShipStats ship)
    {
        ship.currentShielding -= shieldStrength;
        base.RemoveFromShip(ship);
    }
}
