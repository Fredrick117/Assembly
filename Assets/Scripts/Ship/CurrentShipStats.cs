using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CurrentShipStats : MonoBehaviour
{
    public static CurrentShipStats Instance;

    public ShipBaseStats baseStats;

    public UnityEvent onStatsChanged;

    public List<Subsystem> subsystems = new();

    public SubsystemListPanel subsystemListPanel;

    public float currentSublightSpeed;
    public float currentSpeed;
    public float currentPowerDraw;
    public float currentMaxPower;
    public int currentMass;
    public int currentShielding;
    public int currentArmorRating;
    public ShipClassification currentClass;
    public int currentMaxCraft;
    public int currentCrew;
    public int subsystemSlots;
    public int reactorSlots;
    public int weaponSlots;
    public bool canEnterAtmosphere = false;
    public int currentPrice;

    private void Awake()
    {
        Instance = this;
    }

    public void RemoveSubsystem(Subsystem subsystem)
    {
        if (subsystems.Remove(subsystem))
        {
            subsystem.RemoveFromShip(this);
            onStatsChanged?.Invoke();
        }
    }

    public void AddSubsystem(Subsystem subsystem)
    {
        subsystems.Add(subsystem);
        subsystem.ApplyToShip(this);
        onStatsChanged?.Invoke();
    }

    public void ClearCurrentShipStats()
    {
        currentArmorRating = 0;
        currentSublightSpeed = 0;
        currentSpeed = 0;
        currentPowerDraw = 0;
        currentMaxPower = 0;
        currentMass = 0;
        currentClass = 0;
        currentMaxCraft = 0;
        currentCrew = 0;
        currentPrice = 0;
    }

    public void SetBaseStats(ShipBaseStats newBaseStats)
    {
        currentMass = newBaseStats.baseMass;
        currentClass = newBaseStats.shipClass;
        subsystemSlots = newBaseStats.utilitySlots;
        weaponSlots = newBaseStats.weaponSlots;
        reactorSlots = newBaseStats.reactorSlots;
        currentPrice = newBaseStats.basePrice;

        baseStats = newBaseStats;
    }

    public void ClearSubsystems()
    {
        subsystems.Clear();

        foreach (GameObject subsystem in GameObject.FindGameObjectsWithTag("Subsystem"))
        {
            Destroy(subsystem);
        }
    }

    public int GetHighestArmorRating()
    {
        var allArmor = subsystems.Where(s => s is Armor).ToList();

        int highestArmorRating = 0;

        foreach (Subsystem subsystem in allArmor)
        {
            Armor armor = (Armor)subsystem;
            if (armor.armorRating > highestArmorRating)
                highestArmorRating = armor.armorRating;
        }

        return highestArmorRating;
    }

    public bool HasThrusters()
    {
        return subsystems.OfType<Thrusters>().Any();
    }
}
