using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum ShipClass
{
    Cargo,
    Yacht,
    //Shuttle,
    //Prison,
    Research,
    Corvette,
    Destroyer,
    LightCruiser,
    HeavyCruiser,
    Carrier,
    Battleship,
    Dreadnought
}

[System.Serializable]
public struct RequestData
{
    public Species customerSpecies;
    public ShipClass shipClass;
    public ArmorMaterial armorMaterial;
    public WeaponType preferredWeaponType;
    //public int budget;
    //public int minSpeed;
    public RequestedSpeed requestedSpeed;
    public int minHullPoints;
    //public int maxPowerOutput;
    public bool isUnarmed;
    public bool isAtmosphereCapable;
    public bool isAutonomous;
    public bool isFtlCapable;
}

public enum Species
{ 
    Human,
    Arachnid,
    Vynotian,
}

public enum ContractContext
{
    Military,
    Civilian,
}

public enum DamageType
{
    Laser,
    Plasma,
    Electric,
    Kinetic,
    Explosive,
}

public enum RequestInequality
{
    LessThan,
    GreaterThan,
}

public struct RequestedSpeed
{
    public RequestInequality lessOrGreaterThan;
    public int speed;
}