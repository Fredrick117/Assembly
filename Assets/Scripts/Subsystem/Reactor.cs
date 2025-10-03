using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum ReactorType
{
    Fusion,
    Fission,
    Antimatter,
    Solar,
    Geothermal,
    Nuclear
}

[CreateAssetMenu(fileName = "NewReactor", menuName = "Subsystem/Reactor")]
public class Reactor : Subsystem
{
    public float powerOutput;
    public ReactorType reactorType;

    public override void ApplyToShip(ShipStats ship)
    {
        ship.currentMaxPower += powerOutput;
        base.ApplyToShip(ship);
    }

    public override void RemoveFromShip(ShipStats ship)
    {
        ship.currentMaxPower -= powerOutput;
        base.RemoveFromShip(ship);
    }
}
