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
        currentShip.GetComponent<CurrentShipStats>().baseStats = null;
        currentShip.GetComponent<SpriteRenderer>().sprite = null;

        CurrentShipStats.Instance.ClearCurrentShipStats();
        CurrentShipStats.Instance.ClearSubsystems();
        CurrentShipStats.Instance.onStatsChanged?.Invoke();
    }

    public void SetNewShipClass(ShipBaseStats stats)
    {
        CurrentShipStats ship = currentShip.GetComponent<CurrentShipStats>();

        ship.ClearCurrentShipStats();
        ship.ClearSubsystems();
        ship.SetBaseStats(stats);
        currentShip.GetComponent<SpriteRenderer>().sprite = currentShip.GetComponent<CurrentShipStats>().baseStats.baseSprite;

        ship.onStatsChanged?.Invoke();
    }
}
