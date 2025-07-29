using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum ThrusterType
{ 
    SolidPropellant,
    LiquidPropellant,
    Ion,
    NuclearThermal,
    Magic,
}

[CreateAssetMenu(fileName = "NewThrusters", menuName = "Subsystem/Thrusters")]
public class Thrusters : Subsystem
{
    public int speed;
    public bool atmosphericEntryCapable;
    public ThrusterType thrusterType;

    public override void ApplyToShip(ShipStats ship)
    {
        base.ApplyToShip(ship);
        ship.Speed += speed;
    }

    public override void RemoveFromShip(ShipStats ship)
    {
        base.RemoveFromShip(ship);
        ship.Speed -= speed;
    }
}
