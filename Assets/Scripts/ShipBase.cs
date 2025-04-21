using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipBase : MonoBehaviour
{
    //public ShipClass shipClass = 0;
    //public float armor = 0f;
    //public float shielding = 0f;
    //public float hullRating = 0f;
    //public List<Subsystem> subsystems = new List<Subsystem>();
    //public int subsystemSlots = 0;
    //public float totalPowerDraw = 0f;
    //public float maxSpeed = 0f;
    //public float minSpeed = 0f;

    public static ShipBase Instance;

    public BaseShipStats baseStats;
    private int currentArmor { get; set; }
    private int currentHull { get; set; }
    private float currentSpeed { get; set; }
    private float currentPower { get; set; }
    private int currentMass { get; set; }
    private int currentShielding { get; set; }

    public List<Subsystem> subsystems = new List<Subsystem>();

    private void Awake()
    {
        Instance = this;
    }

    public void SetBaseStats()
    {
        currentArmor = baseStats.baseArmor;
        currentHull = baseStats.baseHull;
        currentSpeed = baseStats.baseSpeed;
        currentPower = baseStats.basePower;
    }

    public void ModifyArmor(int amount)
    {
        currentArmor += amount;
    }

    public void ModifyHull(int amount)
    {
        currentHull += amount;
    }    

    public void ModifySpeed(float amount)
    {
        currentSpeed += amount;
    }

    public void ModifyPower(float amount)
    {
        currentPower += amount;
    }

    public void ModifyShielding(int amount)
    {
        currentShielding += amount;
    }
}
