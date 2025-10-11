using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum AtmosphereType
{
    NitrogenOxygen,
    Methane,
    CarbonDioxide,
}

[CreateAssetMenu(fileName = "NewLifeSupport", menuName = "Subsystem/LifeSupport")]
public class LifeSupport : Subsystem
{
    public AtmosphereType atmosphereType;
    public int crew;

    public override void ApplyToShip(ShipStats ship)
    {
        ship.currentCrew += crew;
        base.ApplyToShip(ship);
    }

    public override void RemoveFromShip(ShipStats ship)
    {
        ship.currentCrew -= crew;
        base.RemoveFromShip(ship);
    }
}
