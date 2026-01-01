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
        shipClassText.text = CurrentShipStats.Instance.baseStats != null ? 
            CurrentShipStats.Instance.baseStats.shipClass.ToString() : "None";

        shipArmorText.text = Utilities.ArmorRatingToString(CurrentShipStats.Instance.currentArmorRating);

        bool hasAdequatePower = CurrentShipStats.Instance.currentPowerDraw > CurrentShipStats.Instance.currentMaxPower;
        string powerString = $"{CurrentShipStats.Instance.currentPowerDraw}/{CurrentShipStats.Instance.currentMaxPower} MW";

        if (hasAdequatePower)
            shipPowerText.text = $"<color=red>{powerString}</color>";
        else
            shipPowerText.text = powerString;

        shipMaxSpeedText.text = CurrentShipStats.Instance.currentSpeed.ToString();
        shipMassText.text = CurrentShipStats.Instance.currentMass.ToString();
        shipShieldText.text = CurrentShipStats.Instance.currentShielding.ToString();
        shipCraftText.text = CurrentShipStats.Instance.currentMaxCraft.ToString();
        shipCrewText.text = CurrentShipStats.Instance.currentCrew.ToString();
        shipSublightSpeedText.text = CurrentShipStats.Instance.currentSublightSpeed.ToString();
        string formattedPrice = CurrentShipStats.Instance.currentPrice.ToString("N0");
        shipPriceText.text = $"₡{formattedPrice}";
    }
}
