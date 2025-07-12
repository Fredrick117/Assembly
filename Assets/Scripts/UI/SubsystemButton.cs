using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SubsystemButton : MonoBehaviour, IPointerClickHandler
{
    public Subsystem subsystemData;

    private TMP_Text descriptorText;

    private TMP_Text descriptionText;

    private TMP_Text statsText;

    private Image icon;

    private SubsystemSelectionMenu menuRef;

    void Start()
    {
        menuRef = transform.parent.transform.parent.GetComponent<SubsystemSelectionMenu>();

        descriptorText = transform.GetChild(1).transform.GetChild(0).GetComponent<TMP_Text>();
        descriptionText = transform.GetChild(1).transform.GetChild(1).GetComponent<TMP_Text>();
        statsText = transform.GetChild(1).transform.GetChild(2).GetComponent<TMP_Text>();

        descriptorText.text = subsystemData.displayName;

        if (subsystemData.description.Length > 0)
            descriptionText.text = subsystemData.description;
        else
            descriptionText.text = "No description available.";

        statsText.text = $"Mass: {subsystemData.mass}\tPower draw: {subsystemData.powerDraw}\t";

        if (subsystemData is Reactor reactorData)
            statsText.text += $"Power output: {reactorData.powerOutput} GW\tPower type: {reactorData.powerType}\t";
        if (subsystemData is Shielding shieldData)
            statsText.text += $"Shield strength: {shieldData.shieldStrength}/100\tRecharge speed: {shieldData.rechargeSpeed}%/second\t";
        if (subsystemData is FTLDrive ftl)
            statsText.text += $"Grade: {ftl.grade}";
        if (subsystemData is Armor armor)
            statsText.text += $"Armor rating: {armor.rating}\tMass increase: {armor.massIncrease}\t";
        if (subsystemData is Thrusters thrusters)
            statsText.text += $"Speed increase: {thrusters.maxSpeed}\t Capable of atmospheric entry: {thrusters.atmosphericEntryCapable}\t";

        icon = transform.GetChild(0).GetComponent<Image>();
        icon.sprite = subsystemData.icon;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        ShipStats.Instance.AddSubsystem(menuRef.selectedSlot, subsystemData);
        menuRef.gameObject.SetActive(false);
    }
}
