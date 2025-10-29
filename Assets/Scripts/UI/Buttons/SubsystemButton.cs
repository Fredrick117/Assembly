using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SubsystemButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Subsystem subsystemData;

    private TMP_Text descriptorText;

    private TMP_Text descriptionText;

    private TMP_Text statsText;

    private Image icon;

    private SubsystemSelectionMenu menuRef;

    [SerializeField]
    private Image background;

    void Start()
    {
        background = GetComponent<Image>();

        menuRef = transform.parent.transform.parent.GetComponent<SubsystemSelectionMenu>();

        descriptorText = transform.GetChild(1).transform.GetChild(0).GetComponent<TMP_Text>();
        descriptionText = transform.GetChild(1).transform.GetChild(1).GetComponent<TMP_Text>();
        statsText = transform.GetChild(1).transform.GetChild(2).GetComponent<TMP_Text>();

        descriptorText.text = subsystemData.displayName;

        if (subsystemData.description.Length > 0)
            descriptionText.text = subsystemData.description;
        else
            descriptionText.text = "No description available.";

        statsText.text = $"Mass: +{subsystemData.mass}\tPower draw: {subsystemData.powerDraw}\t";
        statsText.text += $"Price: ${subsystemData.price}\t";

        if (subsystemData is Reactor reactorData)
            statsText.text += $"Power output: +{reactorData.powerOutput} MW\tPower type: {reactorData.reactorType}";
        if (subsystemData is Shielding shieldData)
            statsText.text += $"Shield strength: +{shieldData.shieldStrength}\t";
        if (subsystemData is FTLDrive ftl)
            statsText.text += $"Grade: {ftl.tier}";
        if (subsystemData is Thrusters thrusters)
            statsText.text += $"Speed: +{thrusters.speed} m/s\t";
        if (subsystemData is HangarBay hangarBay)
            statsText.text += $"Maximum spacecraft capacity: +{hangarBay.maxCraft}";
        if (subsystemData is Armor armor)
            statsText.text += $"Armor rating: {Utilities.ArmorRatingToString(armor.armorRating)}\t Atmosphere capable: {armor.canEnterAtmosphere}";
        if (subsystemData is Weapon weapon)
            statsText.text += $"Damage type: {weapon.weaponType}";

        icon = transform.GetChild(0).GetComponent<Image>();
        icon.sprite = subsystemData.icon;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        ShipStats.Instance.AddSubsystem(menuRef.GetSelectedSlot(), subsystemData);
        menuRef.gameObject.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        background.color = new Color(0, 0, 0, 0.3f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        background.color = new Color(0, 0, 0, 0.1f);
    }
}
