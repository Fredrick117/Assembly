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
    Escort
    //Moonbreaker,
}

[System.Serializable]
public struct RequestData	// TODO: rename?
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

    public float minSublightSpeed;
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
    Escort,
    Piracy,
    ShipToShip
}

[System.Serializable]
public class ShipData
{
    public ShipClassification classification;
    public float sublightSpeed;
    public float speed;
    public float powerDraw;
    public float maxPower;
    public float mass;
    public int shieldRating;
    public int armorRating;
    public int maxSpacecraft;
    public int maxCrew;
    public int weaponMounts;
    public int priceToBuild;
}