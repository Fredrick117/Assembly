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
    private TMP_Text shipPowerConsumptionText;
    [SerializeField]
    private TMP_Text shipPowerOutputText;
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
        shipClassText.text = ShipStats.Instance.baseStats.shipClass;
        shipArmorText.text = ShipStats.Instance.Armor.ToString();
        shipHullText.text = ShipStats.Instance.Hull.ToString();
        shipPowerConsumptionText.text = $"{ShipStats.Instance.PowerDraw}";
        shipPowerOutputText.text = $"{ShipStats.Instance.MaxPower}";
        shipMaxSpeedText.text = ShipStats.Instance.Speed.ToString();
        shipMassText.text = ShipStats.Instance.Mass.ToString();
        shipShieldText.text = ShipStats.Instance.ShieldStrength.ToString();
    }
}
