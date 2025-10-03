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
    public int currentMaxStarfighters;
    public int subsystemSlots;
    public int reactorSlots;
    public int weaponSlots;

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

        subsystemData.ApplyToShip(this);

        subsystems[index] = subsystemData;

        subsystemListPanel.slots[index].transform.GetChild(0).gameObject.GetComponent<Image>().sprite = subsystemData.icon;
        subsystemListPanel.slots[index].transform.GetChild(1).gameObject.GetComponent<TMP_Text>().text = subsystemData.displayName;

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
        currentMaxStarfighters = 0;
    }

    public void SetBaseStats(ShipBaseStats newBaseStats)
    {
        currentMass = newBaseStats.baseMass;
        currentClass = newBaseStats.shipClass;
        subsystemSlots = newBaseStats.utilitySlots;
        weaponSlots = newBaseStats.weaponSlots;
        reactorSlots = newBaseStats.reactorSlots;

        baseStats = newBaseStats;
    }

    public void ClearSubsystems()
    {
        subsystems.Clear();
        subsystemListPanel.UpdateSubsystemSlots();
    }
}
