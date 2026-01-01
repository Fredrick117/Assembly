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

        menuRef = transform.parent.parent.parent.GetComponent<SubsystemSelectionMenu>();

        descriptorText = transform.GetChild(1).transform.GetChild(0).GetComponent<TMP_Text>();
        descriptionText = transform.GetChild(1).transform.GetChild(1).GetComponent<TMP_Text>();
        statsText = transform.GetChild(1).transform.GetChild(2).GetComponent<TMP_Text>();

        descriptorText.text = subsystemData.displayName;
        descriptionText.text = subsystemData.description.Length > 0 ? subsystemData.description : "No description available.";

        statsText.text = $"Mass: +{subsystemData.mass}\tPower draw: {subsystemData.powerDraw}\t";
        statsText.text += $"Price: ${subsystemData.price}\t";

        switch (subsystemData)
        {
            case Reactor reactorData:
                statsText.text += $"Power output: +{reactorData.powerOutput} MW\tPower type: {reactorData.reactorType}";
                break;
            case Shielding shieldData:
                statsText.text += $"Shield strength: +{shieldData.shieldStrength}\t";
                break;
            case FTLDrive ftl:
                statsText.text += $"Grade: {ftl.tier}";
                break;
            case Thrusters thrusters:
                statsText.text += $"Speed: +{thrusters.speed} m/s\t";
                break;
            case HangarBay hangarBay:
                statsText.text += $"Maximum spacecraft capacity: +{hangarBay.maxCraft}";
                break;
            case Armor armor:
                statsText.text += $"Armor rating: {Utilities.ArmorRatingToString(armor.armorRating)}\t Atmosphere capable: {armor.canEnterAtmosphere}";
                break;
            case Weapon weapon:
                statsText.text += $"Damage type: {weapon.weaponType}";
                break;
        }

        icon = transform.GetChild(0).GetComponent<Image>();
        icon.sprite = subsystemData.icon;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        CurrentShipStats.Instance.AddSubsystem(menuRef.GetSelectedSlot(), subsystemData);
        menuRef.Close();
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
