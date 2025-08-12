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
    private ArmorMaterial currentArmorMaterial = ArmorMaterial.None;
    private ShipClassification currentClass = ShipClassification.None;
    private int maxStarfighters;

    // Properties
    public int Starfighters
    { 
        get { return maxStarfighters; }
        set 
        { 
            if (maxStarfighters != value)
            {
                maxStarfighters = value;
                onStatsChanged.Invoke();
            }
        }
    }
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
    public ArmorMaterial ArmorMaterial
    {
        get { return currentArmorMaterial; }
        set
        {
            if (value != currentArmorMaterial)
            {
                currentArmorMaterial = value;
                onStatsChanged.Invoke();
            }
        }
    }
    public ShipClassification Class
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

        DontDestroyOnLoad(gameObject);
    }

    public void RemoveSubsystem(int index)
    {
        if (subsystems.TryGetValue(index, out var subsystem))
        {
            subsystem.RemoveFromShip(this);
            subsystems.Remove(index);


            subsystemListPanel.slots[index].transform.GetChild(0).gameObject.GetComponent<Image>().sprite = null;
            subsystemListPanel.slots[index].transform.GetChild(1).gameObject.GetComponent<TMP_Text>().text = "";
        }
    }

    public void AddSubsystem(int index, Subsystem subsystemData)
    {
        if (subsystems.ContainsKey(index))
        {
            subsystems[index].RemoveFromShip(this);
            subsystems.Remove(index);
        }

        subsystemData.ApplyToShip(this);

        subsystems[index] = subsystemData;

        subsystemListPanel.slots[index].transform.GetChild(0).gameObject.GetComponent<Image>().sprite = subsystemData.icon;
        subsystemListPanel.slots[index].transform.GetChild(1).gameObject.GetComponent<TMP_Text>().text = subsystemData.displayName;
    }

    public void SetBaseStats()
    {
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
