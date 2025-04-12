using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShipManager : MonoBehaviour
{
    [HideInInspector]
    public GameObject rootModule = null;

    private List<GameObject> shipModules = new List<GameObject>();

    public SubsystemListPanel subsystemListPanel;

    public GameObject currentShip = null;

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
    private GameObject shipStatsContent;
    [SerializeField]
    private TMP_Text noShipSelectedText;

    [SerializeField]
    private GameObject subsystemSlots;

    public static ShipManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void OnEnable()
    {
        //DraggableManager.onPlace += UpdateShipStats;
    }

    private void OnDisable()
    {
        //DraggableManager.onPlace -= UpdateShipStats;
    }

    //public void ClearAllShipModules()
    //{
    //    shipModules.Clear();
    //    rootModule = null;

    //    GameObject[] modules = GameObject.FindGameObjectsWithTag("ShipModule");
    //    foreach (GameObject module in modules)
    //    {
    //        GameObject.Destroy(module);
    //    }
    //}

    public void ClearShip()
    {
        currentShip.GetComponent<ShipBase>().baseStats = null;
        currentShip.GetComponent<SpriteRenderer>().sprite = null;

        ClearShipStats();
    }

    public void SetShipStats(BaseShipStats stats)
    {
        ShipBase ship = currentShip.GetComponent<ShipBase>();
        ship.baseStats = stats;
        ship.SetBaseStats();
        currentShip.GetComponent<ShipBase>().baseStats = stats;
        currentShip.GetComponent<SpriteRenderer>().sprite = currentShip.GetComponent<ShipBase>().baseStats.sprite;

        UpdateShipStats();
    }

    public void RemoveSubsystem(Subsystem subsystem)
    {
        //currentShip.GetComponent<ShipBase>().subsystems.Remove(subsystem);
    }

    private void ClearShipStats()
    {
        shipClassText.text = "???";
        shipArmorText.text = "0";
        shipShieldText.text = "0";
        shipHullText.text = "0";
        shipPowerText.text = "0";
        shipMaxSpeedText.text = "0";
    }

    private void UpdateShipStats()
    {
        subsystemListPanel.UpdateSubsystemSlots();

        BaseShipStats currentShipStats = currentShip.GetComponent<ShipBase>().baseStats;

        shipClassText.text = currentShipStats.shipClass;
        shipArmorText.text = currentShipStats.baseArmor.ToString();
        shipHullText.text = currentShipStats.baseHull.ToString();
        shipPowerText.text = currentShipStats.basePower.ToString();
        shipMaxSpeedText.text = currentShipStats.baseSpeed.ToString();
    }
}
