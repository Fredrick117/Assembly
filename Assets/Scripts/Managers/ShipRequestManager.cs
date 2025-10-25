using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ShipRequestManager : MonoBehaviour
{
    //[HideInInspector]
    //public int attemptsRemaining = 3;
    [HideInInspector]
    public int numSuccesses = 0;

    private int requestNumber = 1;

    public RequestData activeShipRequest = new RequestData();

    delegate void CreateNewRequest(RequestData Request);
    CreateNewRequest createNewShipRequest;

    public TMP_Text requestText;

    public TMP_Text headerText;

    public Canvas mainCanvas;

    public FeedbackPanel feedbackPanel;

    private NoShipSelectedText noShipSelectedText;

    private static List<ShipBaseStats> shipBaseStatsList = new List<ShipBaseStats>();

    private static List<Subsystem> subsystemsList = new List<Subsystem>();

    private static List<Thrusters> thrusterSubsystems = new();
    private static List<Armor> armorSubsystems = new();

    [SerializeField]
    private ShipGenerator shipGenerator;

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
        thrusterSubsystems = Resources.LoadAll<Thrusters>("ScriptableObjects/Subsystems").ToList();
        armorSubsystems = Resources.LoadAll<Armor>("ScriptableObjects/Subsystems").ToList();

        noShipSelectedText = mainCanvas.GetComponentInChildren<NoShipSelectedText>();
        
        LoadScriptableObjects();

        SetNewRequest();
        SetRequestText();
    }

    private void OnEnable()
    {
        EventManager.onSubmit += OnSubmission;
    }

    private void OnDisable()
    {
        EventManager.onSubmit -= OnSubmission;
    }

    public void SetNewRequest()
    {
        Ship shipFoundation = shipGenerator.GenerateShip();
        activeShipRequest = GetRequestDataFromShip(shipFoundation);

        SetRequestText();
    }

    private RequestData GetRequestDataFromShip(Ship ship)
    {
        RequestData request = new();

        Armor armor = (Armor)ship.subsystems.FirstOrDefault(a => a is Armor);
        if (armor != null)
        {
            request.isAtmosphereCapable = armor.canEnterAtmosphere;
        }
        else
        {
            Debug.LogError("ShipRequestManager: no thrusters :(");
        }

        request.minSpeed = thrusterSubsystems[Random.Range(0, thrusterSubsystems.Count())].speed;
        request.isFtlCapable = ship.isFTL;
        request.isAutonomous = ship.isAutonomous;
        request.isAtmosphereCapable = ship.isAtmospheric;
        request.shipClass = ship.classification;
        request.armorRating = ship.armorRating;

        Shielding shields = (Shielding)ship.subsystems.FirstOrDefault(s => s is Shielding);
        if (shields != null)
        {
            request.minShieldStrength = shields.shieldStrength;
        }

        request.reward = GetRewardAmount(request.shipClass);

        return request;
    }

    private int GetRewardAmount(ShipClassification classification)
    {
        int baseStatsPrice = shipBaseStatsList.First(baseStat => baseStat.shipClass == classification).basePrice;

        return Mathf.RoundToInt(baseStatsPrice * 1.6f);
    }

    private void LoadScriptableObjects()
    {
        shipBaseStatsList = Resources.LoadAll<ShipBaseStats>("ScriptableObjects/Ships").ToList();
        subsystemsList = Resources.LoadAll<Subsystem>("ScriptableObjects/Subsystems").ToList();
    }

    private void OnSubmission()
    {
        bool submissionWasSuccessful = false;

        if (ShipManager.Instance.currentShip.GetComponent<ShipStats>().baseStats == null)
        {
            Debug.LogError("No design submitted!");
            return;
        }

        requestNumber++;

        if (!FulfillsRequirements())
        {
            //attemptsRemaining--;

            feedbackPanel.Show();

            //if (attemptsRemaining <= 0)
            //{
            //    GameManager.Instance.ShowGameOverScreen();
            //}
        }
        else
        {
            submissionWasSuccessful = true;
            numSuccesses++;
        }

        int totalReward = 0;
        totalReward -= ShipStats.Instance.currentPrice;
        
        if (submissionWasSuccessful)
        {
            totalReward += activeShipRequest.reward;
        }

        GameManager.Instance.ModifyCredits(totalReward);

        noShipSelectedText.ShowText();
        SetNewRequest();
        SetRequestText();
        ShipManager.Instance.ClearShip();
    }

    private bool FulfillsRequirements()
    {
        ShipStats ship = ShipManager.Instance.currentShip.GetComponent<ShipStats>();

        bool metSpeedReq = ship.currentSpeed >= activeShipRequest.minSpeed;
        if (!metSpeedReq)
            feedbackPanel.AddSpeedDiscrepancy(ship, activeShipRequest);

        bool metClassReq = activeShipRequest.shipClass.Equals(ship.baseStats.shipClass);
        if (!metClassReq)
            feedbackPanel.AddShipClassDiscrapancy(ship, activeShipRequest);

        //bool metUnarmedReq = activeShipRequest.isArmed == !ship.HasWeapons();
        //if (!metUnarmedReq)
        //    feedbackPanel.AddArmedDiscrepancy(ship, activeShipRequest);
        
        bool metFtlReq = activeShipRequest.isFtlCapable == ship.subsystems.Values.Any(subsystem => subsystem is FTLDrive);
        if (!metFtlReq)
            feedbackPanel.AddFtlDiscrepancy(ship, activeShipRequest);

        List<Subsystem> thrusterSubsystems = ship.subsystems.Values.Where(subsystem => subsystem is Thrusters).ToList();
        bool metAtmosphereReq = activeShipRequest.isAtmosphereCapable == armorSubsystems.Any(armor => ((Armor)armor).canEnterAtmosphere);
        if (!metAtmosphereReq)
            feedbackPanel.AddAtmosphereDiscrepancy(ship, activeShipRequest);

        bool metAutonomousReq = activeShipRequest.isAutonomous == ship.subsystems.Values.Any(subsystem => subsystem is ArtificialIntelligence);
        if (!metAutonomousReq)
            feedbackPanel.AddAiDiscrepancy(ship, activeShipRequest);

        bool metArmorReq = ship.currentArmorRating >= activeShipRequest.armorRating;
        if (!metArmorReq)
            feedbackPanel.AddArmorDiscrepancy(ship, activeShipRequest);

        bool shipHasAdequatePower = ship.currentPowerDraw <= ship.currentMaxPower;
        if (!shipHasAdequatePower)
            feedbackPanel.AddPowerDiscrepancy(ship);

        return metSpeedReq && metClassReq && metFtlReq && metAtmosphereReq && metAutonomousReq && shipHasAdequatePower && metArmorReq;
    }

    private void SetRequestText()
    {
        headerText.text = $"Request {requestNumber.ToString("D3")}";

        requestText.text = $"Design a {activeShipRequest.shipClass.ToString().ToLower()}-class vessel with:\nA minimum speed of <b>{activeShipRequest.minSpeed} m/s</b>\n";

        if (activeShipRequest.isFtlCapable)
            requestText.text += "Faster-than-light travel capability\n";

        if (activeShipRequest.isAtmosphereCapable)
            requestText.text += "Atmospheric entry systems\n";

        if (activeShipRequest.isAutonomous)
            requestText.text += "A shipborne artificial intelligence\n";

        requestText.text += $"Has an armor rating of at least <b>{Utilities.ArmorRatingToString(activeShipRequest.armorRating)}</b>\n";

        requestText.text += $"Shield strength of at least {activeShipRequest.minShieldStrength}\n";   // What if no shields needed?
    }
}
