using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    public int currentHull;
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

    public void RemoveSubsystem(int index)
    {
        if (subsystems.TryGetValue(index, out var subsystem))
        {
            subsystems.Remove(index);
            subsystem.RemoveFromShip(this);

            subsystemListPanel.slots[index].transform.GetChild(0).gameObject.GetComponent<Image>().sprite = null;
            subsystemListPanel.slots[index].transform.GetChild(1).gameObject.GetComponent<TMP_Text>().text = "";

            onStatsChanged?.Invoke();
        }
    }

    public void AddSubsystem(int index, Subsystem subsystemData)
    {
        if (subsystems.ContainsKey(index))
        {
            subsystems[index].RemoveFromShip(this);
            subsystems.Remove(index);
        }

        subsystems[index] = subsystemData;

        subsystemListPanel.slots[index].transform.GetChild(0).gameObject.GetComponent<Image>().sprite = subsystemData.icon;
        subsystemListPanel.slots[index].transform.GetChild(1).gameObject.GetComponent<TMP_Text>().text = subsystemData.displayName;

        subsystemData.ApplyToShip(this);
        onStatsChanged?.Invoke();
    }

    public void ClearShipStats()
    {
        currentArmorRating = 0;
        currentHull = 0;
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
        subsystemListPanel.UpdateSubsystemSlots();
    }

    public int GetHighestArmorRating()
    {
        var allArmor = subsystems.Where(s => s.Value is Armor).ToList();

        int highestArmorRating = 0;

        foreach (KeyValuePair<int, Subsystem> kvp in allArmor)
        {
            Armor armor = (Armor)kvp.Value;
            if (armor.armorRating > highestArmorRating)
                highestArmorRating = armor.armorRating;
        }

        return highestArmorRating;
    }
}
