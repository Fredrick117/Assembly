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

    public void ClearShip()
    {
        currentShip.GetComponent<ShipStats>().baseStats = null;
        currentShip.GetComponent<SpriteRenderer>().sprite = null;

        ShipStats.Instance.ClearShipStats();
    }

    public void SetShipBaseStats(ShipBaseStats stats)
    {
        ShipStats ship = currentShip.GetComponent<ShipStats>();
        ship.baseStats = stats;
        ship.SetBaseStats();
        currentShip.GetComponent<ShipStats>().baseStats = stats;
        currentShip.GetComponent<SpriteRenderer>().sprite = currentShip.GetComponent<ShipStats>().baseStats.sprite;
    }
}
