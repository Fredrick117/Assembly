using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ShipRequestManager : MonoBehaviour
{
    public int attemptsRemaining = 3;
    public int numSuccesses = 0;

    public RequestData activeShipRequest = new RequestData();

    delegate void CreateNewRequest(RequestData Request);
    CreateNewRequest createNewShipRequest;

    public TMP_Text requestText;

    public List<ShipBaseStats> shipBaseStats = new List<ShipBaseStats>();

    public static ShipRequestManager Instance { get; private set; }

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


    private void Start()
    {
        SetNewRequest();
    }

    private void OnEnable()
    {
        EventManager.onSubmit += OnSubmission;
    }

    private void OnDisable()
    {
        EventManager.onSubmit -= OnSubmission;
    }

    /// <summary>
    /// Creates a random request for a ship and sets it to the current ship request.
    /// </summary>
    public void SetNewRequest()
    {
        int baseStatIndex = Random.Range(0, shipBaseStats.Count);
        activeShipRequest.shipClass = shipBaseStats[baseStatIndex].shipClass;
        //data.size = Random.Range(0, 4);

        // Speed can be the base speed of the ship +/- 2500 km/h
        float extraSpeed = Random.Range(0, 26) * 100f;
        activeShipRequest.minSpeed = shipBaseStats[baseStatIndex].baseSpeed + extraSpeed;

        //data.unarmed = Random.Range(0, 2) == 0 ? false : true;
        // TODO: until weapons can be added, always unarmed ship
        activeShipRequest.isUnarmed = true;

        int extraArmor = Random.Range(0, 10) * 10;
        activeShipRequest.minArmor = shipBaseStats[baseStatIndex].baseArmor + extraArmor;

        int extraPower = Random.Range(0, 100) * 10;
        activeShipRequest.minPower = shipBaseStats[baseStatIndex].basePower + extraPower;

        SetRequestText();
    }

    private void OnSubmission()
    {
        if (!FulfillsRequirements())
        {
            attemptsRemaining--;
            if (attemptsRemaining <= 0)
            {
                print("Game over!");
                Application.Quit();
            }
        }
        else
        {
            numSuccesses++;
            attemptsRemaining = 3;

            ShipManager.Instance.ClearShip();
            SetNewRequest();
        }
    }

    private bool FulfillsRequirements()
    {
        ShipStats ship = ShipManager.Instance.currentShip.GetComponent<ShipStats>();
        if (ship.baseStats == null)
        {
            Debug.LogError("No ship detected!");
            return false;
        }

        //bool metSpeedReq = activeShipRequest.minSpeed <= ship.currentSpeed;
        bool metClassReq = activeShipRequest.shipClass.Equals(ship.baseStats.shipClass);
        bool metUnarmedReq = true;  // TODO: change
        bool metArmorReq = activeShipRequest.minArmor <= ship.Armor;
        //bool metPowerReq = activeShipRequest.minPower <= ship.currentMaxPower;

        return /*metSpeedReq && */metClassReq && metUnarmedReq && metArmorReq/* && metPowerReq*/;
    }

    private void SetRequestText()
    {
        requestText.text = "Is a <b>" + activeShipRequest.shipClass.ToString() + "</b>\n\n";
        requestText.text += "Has a speed of at least <b>" + activeShipRequest.minSpeed.ToString() + " m/s</b>\n\n";
        requestText.text += "Is <b>" + (activeShipRequest.isUnarmed ? "unarmed" : "armed") + "</b>\n\n";
        requestText.text += "Has an <b>armor</b> rating of at least <b>" + activeShipRequest.minArmor + "</b>\n\n";
        requestText.text += "Has a total power output of at least <b>" + activeShipRequest.minPower + " GW</b>\n\n";
    }
}
