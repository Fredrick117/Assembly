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

        shipArmorText.text = ShipStats.Instance.ArmorMaterial.ToString();

        bool hasAdequatePower = ShipStats.Instance.PowerDraw > ShipStats.Instance.MaxPower;
        string powerString = $"{ShipStats.Instance.PowerDraw}/{ShipStats.Instance.MaxPower} MW";
        if (hasAdequatePower)
            shipPowerText.text = $"<color=red>{powerString}</color>";
        else
            shipPowerText.text = powerString;

            shipMaxSpeedText.text = ShipStats.Instance.Speed.ToString();
        shipMassText.text = ShipStats.Instance.Mass.ToString();
        shipShieldText.text = ShipStats.Instance.ShieldStrength.ToString();
    }
}
