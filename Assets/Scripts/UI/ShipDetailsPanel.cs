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
    private TMP_Text shipHullText;
    [SerializeField]
    private TMP_Text shipPowerText;
    [SerializeField]
    private TMP_Text shipMassText;

    private void Start()
    {
        ShipStats.Instance.onStatsChanged.AddListener(UpdateText);
    }

    private void OnDisable()
    {
        ShipStats.Instance.onStatsChanged.RemoveListener(UpdateText);
    }

    public void UpdateText()
    {
        shipClassText.text = ShipStats.Instance.baseStats != null ? 
            ShipStats.Instance.baseStats.shipClass.ToString() : "None";

        shipArmorText.text = ShipStats.Instance.currentArmorRating.ToString();

        bool hasAdequatePower = ShipStats.Instance.currentPowerDraw > ShipStats.Instance.currentMaxPower;
        string powerString = $"{ShipStats.Instance.currentPowerDraw}/{ShipStats.Instance.currentMaxPower} MW";
        if (hasAdequatePower)
            shipPowerText.text = $"<color=red>{powerString}</color>";
        else
            shipPowerText.text = powerString;

        shipMaxSpeedText.text = ShipStats.Instance.currentSpeed.ToString();
        shipMassText.text = ShipStats.Instance.currentMass.ToString();
        shipShieldText.text = ShipStats.Instance.currentShielding.ToString();
    }
}
