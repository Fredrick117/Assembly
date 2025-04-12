using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Subsystem
{
    public string displayName;
    public string description;
    public float power;
    public string type;
    public float mass;
    public string icon;
    //public int rank;
}

[System.Serializable]
public class SubsystemsData
{
    public ReactorSubsystem[] reactors;
    public ShieldGeneratorSubsystem[] shieldGenerators; 
    public LifeSupportSubsystem[] lifeSupports;
}

[System.Serializable]
public class ShieldGeneratorSubsystem : Subsystem
{
    public float strength;
    public float rechargeSpeed;
    public List<string> resistanceTypes;
}

[System.Serializable]
public class ReactorSubsystem : Subsystem
{
    public float powerOutput;
    public string powerType;
    public float efficiency;
}

[System.Serializable]
public class LifeSupportSubsystem : Subsystem
{
    public int crewCapacity;
    public string lifeSupportType;
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
    //public float maxSpeed;
    public string shipClass;
    public bool unarmed;
    public HashSet<string> requiredSubsystems;
    public int size;    // 1 = small (corvette, destroyer), 2 = large (cruisers), 3 = massive (carrier, battleship, dreadnought)
    //public string damageType;

    //public int reward;
}
