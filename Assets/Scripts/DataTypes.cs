using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public enum SubsystemType
{
    REACTOR,
    LIFE_SUPPORT,
    SHIELD_GENERATOR,
    NAVIGATION,
    AI,
    WARP_DRIVE,
}

//public enum ShipClass
//{ 
//    Hammerhead,
//    Sword,
//    Titan,
//}

public enum ReactorClass
{
    Apollo,
    Zeus,
}

[CreateAssetMenu(fileName = "New Subsystem", menuName = "Ship Parts/Subsystem")]
public class Subsystem : ScriptableObject
{
    public string subsystemName;
    public string subsystemDescription;
    public SubsystemType subsystemType;
}

public class ShipRequest
{
    public ShipClass? shipClass { get; set; }
    public int? maxWeapons {get; set;}
    public int? minWeapon {get; set;}
    public int budget {get; set;}
    public float minSpeed {get; set;}
    public float maxSpeed {get; set;}
    public List<Subsystem> requiredSubsystems {get; set;}
}
