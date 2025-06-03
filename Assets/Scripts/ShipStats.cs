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

    // Fields
    private int currentArmor;
    private int currentHull;
    private float currentSpeed;
    private float currentPowerDraw;
    private float currentMaxPower;
    private int currentMass;
    private int currentShielding;
    private string currentClass;


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
    public string Class
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


    public UnityEvent onStatsChanged;

    public Dictionary<int, Subsystem> subsystems = new Dictionary<int, Subsystem>();

    public SubsystemListPanel subsystemListPanel;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            Debug.LogWarning(subsystems.Count);
        }
    }

    public void RemoveSubsystem(int index)
    {
        if (subsystems.ContainsKey(index))
        {
            Subsystem subsystemData = subsystems[index];

            if (subsystemData is Reactor reactorData)
            {
                MaxPower -= reactorData.powerOutput;
                Mass -= subsystemData.mass;
            }

            if (subsystemData is Shielding shieldData)
            {
                ShieldStrength -= shieldData.shieldStrength;
                Mass -= shieldData.mass;
                PowerDraw -= shieldData.powerDraw;
            }

            subsystems.Remove(index);
        }
    }

    public void AddSubsystem(int index, Subsystem subsystemData)
    {
        RemoveSubsystem(index);

        subsystems[index] = subsystemData;

        if (subsystemData is Reactor reactorData)
        {
            MaxPower += reactorData.powerOutput;
            Mass += subsystemData.mass;
        }

        if (subsystemData is Shielding shieldData)
        {
            ShieldStrength += shieldData.shieldStrength;
            Mass += shieldData.mass;
            PowerDraw += shieldData.powerDraw;
        }

        //subsystemListPanel.slots[index].transform.GetChild(0).gameObject.GetComponent<Image>().sprite = subsystemData.icon;
        subsystemListPanel.slots[index].transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text = subsystemData.displayName;

        foreach (KeyValuePair<int, Subsystem> pair in subsystems)
        {
            print($"index: {pair.Key}, subsystem: {pair.Value.displayName}");
        }
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
        Class = "";
    }
}
