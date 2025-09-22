using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum ShipType
{
    Warship,
    Exploration,
    Cargo,
}

[System.Serializable]
public enum ShipClass
{ 
    Corvette,
    Destroyer,
    Carrier
}

[System.Serializable]
public struct ShipRequestData
{
    public int budget;
    public float minSpeed;
    public float maxSpeed;
    public ShipType shipType;
    public ShipClass shipClass;
    public HashSet<Subsystem> requiredSubsystems;
}

[System.Serializable]
public enum Affiliation
{
    UnitedEarthNations,
    UnitedMartianNations,
    LunaCorp,
    Pirate,
}

[System.Serializable]
public struct ShipContract
{
    public string customerName;
    public Affiliation customerAffiliation;
    public string description;
    public ShipRequestData data;
}
