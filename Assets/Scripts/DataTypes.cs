using System;
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
    public int maxSpeed;

    public bool isAtmosphereCapable;

    public bool isAutonomous;

    public bool isFtlCapable;

    public int minShieldStrength;

    public int minArmorRating;

    public int budget;
    public int reward;

    public ShipRole roles;

    public int minDamagePerSecond;
    public int maxDamagePerSecond;

    public int minCrew;
    public int maxCrew;

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

public enum ShipRole
{
    Ship_To_Ship,
    Recon,
    Transport,
    Escort,
    Patrol,
    Carrier,
    Enforcement
}

// Used for when ship designs are stored after submission
public class StarshipData
{
    public int hull;
    public int speed;
    public int powerDraw;
    public int maxPower;
    public int mass;
    public int shielding;
    public int armor;
    public ShipClassification shipClass;
}