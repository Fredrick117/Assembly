using System;
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
    public int price;
    public Sprite icon;
    public ItemData itemData;   // TODO: combine ItemData and Subsystem

    public virtual void ApplyToShip(CurrentShipStats ship)
    {
        ship.currentMass += mass;
        ship.currentPowerDraw += powerDraw;
        ship.currentPrice += price;
    }

    public virtual void RemoveFromShip(CurrentShipStats ship)
    {
        ship.currentMass -= mass;
        ship.currentPowerDraw -= powerDraw;
        ship.currentPrice -= price;
    }
}