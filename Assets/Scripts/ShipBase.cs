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
    public event Action OnShipUpdated;

    public BaseShipStats baseStats;
    private int currentArmor;
    private int currentHull;
    private float currentSpeed;
    private float totalPower;

    private void Awake()
    {
        Instance = this;
    }

    public void SetBaseStats()
    {
        currentArmor = baseStats.baseArmor;
        currentHull = baseStats.baseHull;
        currentSpeed = baseStats.baseSpeed;
        totalPower = baseStats.basePower;
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
        totalPower += amount;
    }
}
