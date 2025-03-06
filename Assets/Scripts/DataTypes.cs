using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum Subsystem
{
    Reactor,
    Navigation,
    LifeSupport,
    Shields,
    AI,
    FTLDrive
}

[System.Serializable]
public enum DamageType
{ 
    Kinetic,
    Energy
}

[System.Serializable]
public enum ShipType
{
    Military,
    Civilian
}

[System.Serializable]
public enum ShipClass
{ 
    Corvette,
    Destroyer,
    Cruiser,
    Science,
    Construction,
    Cargo
}

[System.Serializable]
public struct RequestData
{
    //public int? budget;
    public float minSpeed;
    public float maxSpeed;
    public ShipType shipType;
    public ShipClass shipClass;
    public HashSet<Subsystem> requiredSubsystems;
    public DamageType damageType;

    //public int reward;
}

[System.Serializable]
public class Starship
{ 
    public ShipType type;
    public ShipClass classification;
    public List<Subsystem> subsystems;
}
