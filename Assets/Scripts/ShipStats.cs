using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ShipStats : MonoBehaviour
{
    public static ShipStats Instance;

    public ShipBaseStats baseStats;

    public UnityEvent onStatsChanged;

    public Dictionary<int, Subsystem> subsystems = new();

    public SubsystemListPanel subsystemListPanel;

    // Fields
    private int currentArmor;
    private int currentHull;
    private float currentSpeed;
    private float currentPowerDraw;
    private float currentMaxPower;
    private int currentMass;
    private int currentShielding;
    private ShipClass currentClass;

    // Properties
    public int Armor
    { 
        get { return currentArmor; }
        set
        {
            if (currentArmor != value)
            {
                currentArmor = value;
                onStatsChanged.Invoke();
            }
        }
    }
    public int Mass
    { 
        get { return currentMass; }
        set
        {
            if (currentMass != value)
            {
                
                currentMass = value;
                onStatsChanged.Invoke();
            }
        }
    }
    public int Hull
    {
        get { return currentHull; }
        set
        {
            if (currentHull != value)
            {

                currentHull = value;
                onStatsChanged.Invoke();
            }
        }
    }
    public float Speed
    {
        get { return currentSpeed; }
        set
        {
            if (currentSpeed != value)
            {

                currentSpeed = value;
                onStatsChanged.Invoke();
            }
        }
    }
    public float PowerDraw
    {
        get { return currentPowerDraw; }
        set 
        {
            if (currentPowerDraw != value)
            {
                currentPowerDraw = value;
                onStatsChanged.Invoke();
            }
        }
    }
    public float MaxPower
    {
        get { return currentMaxPower; }
        set 
        {
            if (currentMaxPower != value)
            {
                currentMaxPower = value;
                onStatsChanged.Invoke();
            }
        }
    }
    public int ShieldStrength
    { 
        get { return currentShielding; }
        set 
        { 
            if (currentShielding != value)
            {
                currentShielding = value;
                onStatsChanged.Invoke();
            }
        }
    }
    public ShipClass Class
    {
        get { return currentClass; }
        set
        {
            if (value != currentClass)
            {
                currentClass = value;
                onStatsChanged.Invoke();
            }
        }
    }

    private void Awake()
    {
        Instance = this;
    }

    public void RemoveSubsystem(int index)
    {
        if (subsystems.TryGetValue(index, out var subsystem))
        {
            subsystem.RemoveFromShip(this);
            subsystems.Remove(index);
        }
    }

    public void AddSubsystem(int index, Subsystem subsystemData)
    {
        subsystemData.ApplyToShip(this);

        subsystems[index] = subsystemData;

        //subsystemListPanel.slots[index].transform.GetChild(0).gameObject.GetComponent<Image>().sprite = subsystemData.icon;
        subsystemListPanel.slots[index].transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text = subsystemData.displayName;
    }

    public void SetBaseStats()
    {
        Armor = baseStats.baseArmor;
        Hull = baseStats.baseHull;
        Speed = baseStats.baseSpeed;
        PowerDraw = baseStats.basePower;
        MaxPower = 0;
        Mass = baseStats.baseMass;
        Class = baseStats.shipClass;
    }

    public void ClearShipStats()
    {
        Armor = 0;
        Hull = 0;
        Speed = 0;
        PowerDraw = 0;
        MaxPower = 0;
        Mass = 0;
        Class = 0;
    }

    public bool HasWeapons()
    {
        foreach(var subsystem in subsystems.Values)
        {
            if (subsystem is Weapon weapon)
            {
                return true;
            }
        }

        return false;
    }

    public void ClearSubsystems()
    {
        subsystems.Clear();
        subsystemListPanel.UpdateSubsystemSlots();
        onStatsChanged.Invoke();
    }
}
