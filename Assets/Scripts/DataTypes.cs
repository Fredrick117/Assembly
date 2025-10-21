using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum ShipClassification
{
    None,
    //Cargo,
    //Yacht,
    //Research,
    //Corvette,
    //Destroyer,
    //LightCruiser,
    //HeavyCruiser,
    //Battleship,
    //Dreadnought
    Corvette,
    Destroyer,
    Cruiser,
    Carrier,
    //Moonbreaker,
}

[System.Serializable]
public struct RequestData
{
    public ShipClassification shipClass;
    public int minSpeed;
    //public bool isArmed;
    public bool isAtmosphereCapable;
    public bool isAutonomous;
    public bool isFtlCapable;
    public int minShieldStrength;
    public int armorRating;
    public int reward;

    public List<DamageType> preferredDamageTypes;
}

public enum DamageType
{
    Laser,
    Plasma,
    Electric,
    Kinetic,
    Explosive,
}

public enum ArmorRating
{
    None,
    D,
    C,
    B,
    A,
    S
}