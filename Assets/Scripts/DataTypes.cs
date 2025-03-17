using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Subsystem
{
    public string displayName;
    public string description;
    public float power;
    public SubsystemType type;
    public float mass;
    public Sprite icon;
    public int rank;
}

[System.Serializable]
public class ShieldGeneratorSubsystem : Subsystem
{
    public float strength;
    public float rechargeSpeed;
    public List<DamageType> resistanceTypes;
}

[System.Serializable]
public class ReactorSubsystem : Subsystem
{
    public float powerOutput;
    public PowerType powerType;
    public float efficiency;
}

[System.Serializable]
public class LifeSupportSubsystem : Subsystem
{
    public int crewCapacity;
    public LifeSupportType lifeSupportType;
}

[System.Serializable]
public enum LifeSupportType
{ 
    Oxygen,
    Methane,
    CarbonDioxide,
}

[System.Serializable]
public enum PowerType
{ 
    Fission,
    Fusion,
    BlackHole,
    Solar,
    Antimatter,
    DarkMatter,
    ZeroPoint,
}

[System.Serializable]
public enum SubsystemType
{
    Reactor,
    ShieldGenerator,
    LifeSupport,
    //AI
    //Cloaking
}

[System.Serializable]
public enum DamageType
{ 
    Kinetic,
    Energy,
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
    LightCruiser,
    HeavyCruiser,
    Carrier,
    Battleship,
    Dreadnought
}

[System.Serializable]
public struct RequestData
{
    //public int? budget;
    public float minSpeed;
    public float maxSpeed;
    public ShipType shipType;
    public ShipClass shipClass;
    public HashSet<SubsystemType> requiredSubsystems;
    public DamageType damageType;

    //public int reward;
}

[System.Serializable]
public class Starship
{ 
    public ShipType type;
    public ShipClass classification;
    public List<SubsystemType> subsystems;
}
