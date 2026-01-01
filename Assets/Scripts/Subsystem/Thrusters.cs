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
    //public bool atmosphericEntryCapable;
    //public ThrusterType thrusterType;

    public override void ApplyToShip(CurrentShipStats ship)
    {
        ship.currentSpeed += speed;
        base.ApplyToShip(ship);
    }

    public override void RemoveFromShip(CurrentShipStats ship)
    {
        ship.currentSpeed -= speed;
        base.RemoveFromShip(ship);
    }
}
