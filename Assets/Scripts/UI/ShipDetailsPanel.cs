using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class ShipDetailsPanel : MonoBehaviour
{
    [SerializeField]
    private TMP_Text shipMaxSpeedText;
    [SerializeField]
    private TMP_Text shipClassText;
    [SerializeField]
    private TMP_Text shipArmorText;
    [SerializeField]
    private TMP_Text shipShieldText;
    [SerializeField]
    private TMP_Text shipSublightSpeedText;
    [SerializeField]
    private TMP_Text shipPowerText;
    [SerializeField]
    private TMP_Text shipMassText;
    [SerializeField]
    private TMP_Text shipCrewText;
    [SerializeField]
    private TMP_Text shipCraftText;
    [SerializeField]
    private TMP_Text shipPriceText;

    private void Start()
    {
        CurrentShipStats.Instance.onStatsChanged.AddListener(UpdateText);
    }

    private void OnDisable()
    {
        CurrentShipStats.Instance.onStatsChanged.RemoveListener(UpdateText);
    }

    public void UpdateText()
    {
        string formattingText = "<line-height=0>\n<align=right>";
        
        shipClassText.text = "Class:" + formattingText + (CurrentShipStats.Instance.baseStats != null ? 
            CurrentShipStats.Instance.baseStats.shipClass.ToString() : "None"); // ew, fix this

        shipArmorText.text = "Armor rating:" + formattingText + Utilities.ArmorRatingToString(CurrentShipStats.Instance.currentArmorRating);

        bool hasAdequatePower = CurrentShipStats.Instance.currentPowerDraw > CurrentShipStats.Instance.currentMaxPower;
        string powerString = $"Power:{formattingText}{CurrentShipStats.Instance.currentPowerDraw}/{CurrentShipStats.Instance.currentMaxPower} MW";

        if (hasAdequatePower)
            shipPowerText.text = $"<color=red>{powerString}</color>";
        else
            shipPowerText.text = powerString;

        shipMaxSpeedText.text = $"Speed:{formattingText}{CurrentShipStats.Instance.currentSpeed}";
        shipMassText.text = $"Mass:{formattingText}{CurrentShipStats.Instance.currentMass}";
        shipShieldText.text = $"Shield rating{formattingText}{CurrentShipStats.Instance.currentShielding}";
        shipCraftText.text = $"Max ship storage:{formattingText}{CurrentShipStats.Instance.currentMaxCraft}";
        shipCrewText.text = $"Max crew:{formattingText}{CurrentShipStats.Instance.currentCrew}";
        shipSublightSpeedText.text = $"Sublight speed:{formattingText}{CurrentShipStats.Instance.currentSublightSpeed}";
        string formattedPrice = CurrentShipStats.Instance.currentPrice.ToString("N0");
        shipPriceText.text = $"Price:{formattingText}₡{formattedPrice}";
    }
}
