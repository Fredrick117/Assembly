using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSubsystem", menuName = "Subsystem/BaseSubsystem")]
public class Subsystem : ScriptableObject
{
    public string displayName;
    public string description;
    public int powerDraw;
    public int mass;
    //public int price;
    public Sprite icon;

    public virtual void ApplyToShip(ShipStats ship)
    {
        ship.currentMass += mass;
        ship.currentPowerDraw += powerDraw;
    }

    public virtual void RemoveFromShip(ShipStats ship)
    {
        ship.currentMass -= mass;
        ship.currentPowerDraw -= powerDraw;
    }
}