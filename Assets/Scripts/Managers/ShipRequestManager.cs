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

    private int requestNumber = 1;

    public RequestData activeShipRequest = new RequestData();

    delegate void CreateNewRequest(RequestData Request);
    CreateNewRequest createNewShipRequest;

    public TMP_Text requestText;

    public TMP_Text headerText;

    public Canvas mainCanvas;

    public FeedbackPanel feedbackPanel;

    private NoShipSelectedText noShipSelectedText;

    private List<ShipBaseStats> shipBaseStatsList = new List<ShipBaseStats>();

    private List<Subsystem> subsystemsList = new List<Subsystem>();

    private List<Thrusters> thrusterSubsystems = new();

    public static ShipRequestManager Instance { get; private set; }

    //private Dictionary<Species, DamageType> speciesWeaponPreferences = new Dictionary<Species, DamageType> 
    //{ 
    //    { Species.Human, DamageType.Kinetic },
    //    { Species.Vynotian, DamageType.Plasma },
    //    { Species.Arachnid, DamageType.Laser }
    //};

    //private Dictionary<Species, int> speciesMilitaryShipProbability = new Dictionary<Species, int>
    //{
    //    { Species.Human, 67 },
    //    { Species.Vynotian, 30 },
    //    { Species.Arachnid, 90 },
    //};

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
        activeShipRequest = CreateRandomShipRequest();
        SetRequestText();
    }

    private RequestData CreateRandomShipRequest()
    {
        RequestData request = new RequestData();

        request.shipClass = (ShipClassification)Random.Range(1, 4);

        Thrusters minThrusters = thrusterSubsystems[Random.Range(0, thrusterSubsystems.Count)];
        request.minSpeed = minThrusters.speed;
        //request.minSpeed = Random.Range(2, 11) * 100;
        request.isFtlCapable = Utilities.FlipCoin();
        request.isAtmosphereCapable = minThrusters.atmosphericEntryCapable;
        request.isAutonomous = Utilities.FlipCoin();

        return request;
    }

    private void LoadScriptableObjects()
    {
        shipBaseStatsList = Resources.LoadAll<ShipBaseStats>("ScriptableObjects/Ships").ToList();
        subsystemsList = Resources.LoadAll<Subsystem>("ScriptableObjects/Subsystems").ToList(); // is any data lost by doing this?
    }

    private void OnSubmission()
    {
        if (ShipManager.Instance.currentShip.GetComponent<ShipStats>().baseStats == null)
        {
            Debug.LogError("No design submitted!");
            return;
        }

        requestNumber++;

        if (!FulfillsRequirements())
        {
            attemptsRemaining--;

            feedbackPanel.Show();

            if (attemptsRemaining <= 0)
            {
                GameManager.Instance.ShowGameOverScreen();
            }
        }
        else
        {
            numSuccesses++;
        }

        noShipSelectedText.ShowText();
        SetNewRequest();
        SetRequestText();
        ShipManager.Instance.ClearShip();
    }

    private bool FulfillsRequirements()
    {
        ShipStats ship = ShipManager.Instance.currentShip.GetComponent<ShipStats>();

        bool metSpeedReq = ship.Speed >= activeShipRequest.minSpeed;
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
        bool metAtmosphereReq = activeShipRequest.isAtmosphereCapable == thrusterSubsystems.Any(thruster => ((Thrusters)thruster).atmosphericEntryCapable);
        if (!metAtmosphereReq)
            feedbackPanel.AddAtmosphereDiscrepancy(ship, activeShipRequest);

        bool metAutonomousReq = activeShipRequest.isAutonomous == ship.subsystems.Values.Any(subsystem => subsystem is ArtificialIntelligence);
        if (!metAutonomousReq)
            feedbackPanel.AddAiDiscrepancy(ship, activeShipRequest);

        //bool metArmorReq = ship.ArmorMaterial == activeShipRequest.armorMaterial;
        //if (!metArmorReq)
        //    feedbackPanel.AddArmorDiscrepancy(ship, activeShipRequest);

        bool shipHasAdequatePower = ship.PowerDraw <= ship.MaxPower;
        if (!shipHasAdequatePower)
            feedbackPanel.AddPowerDiscrepancy(ship);

        return metSpeedReq && metClassReq && metFtlReq && metAtmosphereReq && metAutonomousReq && shipHasAdequatePower;
    }

    private void SetRequestText()
    {
        headerText.text = $"Request {requestNumber.ToString("D3")}";

        requestText.text = $"Design a {activeShipRequest.shipClass.ToString().ToLower()}-class vessel capable of {activeShipRequest.minSpeed} m/s";
        if (activeShipRequest.isFtlCapable)
            requestText.text += ", with faster-than-light capabilities";
        if (activeShipRequest.isAtmosphereCapable)
            requestText.text += " and atmospheric entry systems";
        requestText.text += ". ";
        if (activeShipRequest.isAutonomous)
            requestText.text += "The vessel should also be autonomous.";
    }
}
