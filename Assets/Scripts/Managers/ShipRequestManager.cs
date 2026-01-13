using System;
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
    public AfterActionReportPanel afterActionReportPanel;

    private NoShipSelectedText noShipSelectedText;

    private static List<ShipBaseStats> shipBaseStatsList = new List<ShipBaseStats>();

    private static List<Subsystem> subsystemsList = new List<Subsystem>();

    private static List<Thrusters> thrusterSubsystems = new();
    private static List<Armor> armorSubsystems = new();

    private readonly Dictionary<string, List<string>> orgNames = new()
    {
        { "United Nations of Earth", new List<string> { "Nathan", "Jonathan", "Terrance", "Alexander", "Bryson", "Malcolm", "Reagan", "Nancy", "Margaret", "Alison", "Amanda", "Rex", "Samantha", "Katrina", "Carlos", "John", "Fred", "Rick", "Richard"} },
        { "Human Empire", new List<string> 
            { 
                "Malachai",
                "Lucilla",
                "Garran",
                "Varek",
                "Elyra",
                "Cassian",
                "Lexum",
                "Kora",
                "Septimus",
                "Rhen",
                "Aurelia",
                "Jast",
                "Malvus",
                "Lyssa",
                "Torvix",
                "Mara",
                "Kalen",
                "Tiber",
                "Daren",
                "Vexa"
            } 
        },
        { "The Dominion", new List<string> 
            {
                "Var'rash",
                "Torkal'Neth",
                "Zhur'ka",
                "Maath",
                "Khaal'tek",
                "Ornak",
                "Shra'ven",
                "Ka'arn",
                "Drek'ra",
                "Vorr'uun",
                "Thre'esh",
                "Graal'Tekh",
                "Nur'vak",
                "Shu'un",
                "Vor'rek",
                "Xel'nor",
                "Korr'vek",
                "Tar'vus",
                "Baal'drun",
                "Kaath'ra",
                "Tor'bex"
            }
        }
    };

    private readonly Dictionary<ShipClassification, Tuple<int, int>> shipSpeedRanges = new()
    {
        { ShipClassification.Corvette, new Tuple<int, int>(1000, 1500) },
        { ShipClassification.Destroyer, new Tuple<int, int>(500, 700) },
        { ShipClassification.Cruiser, new Tuple<int, int>(300, 500) },
        { ShipClassification.Carrier, new Tuple<int, int>(300, 400) },
        { ShipClassification.Escort, new Tuple<int, int>(1000, 1200)}
    };

    private readonly Dictionary<ShipClassification, Tuple<int, int>> shipCrewRanges = new()
    {
        { ShipClassification.Corvette, new Tuple<int, int>(50, 250) },
        { ShipClassification.Destroyer, new Tuple<int, int>(50, 350) },
        { ShipClassification.Cruiser, new Tuple<int, int>(100, 450) },
        { ShipClassification.Carrier, new Tuple<int, int>(200, 1000) },
        { ShipClassification.Escort, new Tuple<int, int>(50, 100) },
    };

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
        LoadScriptableObjects();

        noShipSelectedText = mainCanvas.GetComponentInChildren<NoShipSelectedText>();
        
        activeShipRequest = GenerateNewRequest();
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

    private bool IsShipFunctional(CurrentShipStats currentStats)
    {
        // Does it have thrusters?
        if (!currentStats.subsystems.OfType<Thrusters>().Any())
        {
            Debug.LogError("This ship does not contain any thrusters!");
            return false;
        }
        
        // Does it have life support?
        if (!currentStats.subsystems.OfType<LifeSupport>().Any())
        {
            Debug.LogError("This ship does not contain life support!");
            return false;
        }
        
        // Does it have a reactor (or multiple reactors) that supply enough power to the rest of the ship?
        List<Reactor> reactors = currentStats.subsystems.OfType<Reactor>().ToList();
        float totalReactorOutput = 0f;
        foreach (var reactor in reactors)
        {
            totalReactorOutput += reactor.powerOutput;
        }

        float totalPowerDraw = 0f;
        foreach (var subsystem in currentStats.subsystems)
        {
            totalPowerDraw += subsystem.powerDraw;
        }

        if (totalPowerDraw > totalReactorOutput)
        {
            Debug.LogError("The reactors cannot supply enough power to the rest of the ship!");
            return false;
        }

        Debug.Log("This ship is functional!");
        return true;
    }

    private RequestData GenerateNewRequest()
    {
        RequestData request = new();

        request.shipClass = Utilities.GetRandomEnumValue<ShipClassification>(true);
        request.reward += shipBaseStatsList.First(ship => ship.shipClass == request.shipClass).basePrice;

        Tuple<int, int> speedRange = shipSpeedRanges[request.shipClass];
        request.minSpeed = Mathf.RoundToInt(UnityEngine.Random.Range(speedRange.Item1 / 100, speedRange.Item2 / 100)) * 100;

        bool atmosphereCapable = Utilities.FlipCoin();
        if (atmosphereCapable)
        {
            request.reward += 50000 + UnityEngine.Random.Range(0, 11) * 1000;
            request.isAtmosphereCapable = true;
        }

        bool ftlCapable = Utilities.FlipCoin();
        if (ftlCapable)
        {
            request.reward += 100000;
            request.isFtlCapable = true;
        }

        request.minArmorRating = (int)Utilities.GetRandomEnumValue<ArmorRating>(true);
        request.reward += 1000000;

        bool aiRequired = Utilities.FlipCoin();
        if (aiRequired)
        {
            request.reward += 500000;
            request.isAutonomous = true;
        }

        if (!request.isAutonomous)
        {
            Tuple<int, int> crewRange = shipCrewRanges[request.shipClass];
            request.minCrew = Mathf.RoundToInt(UnityEngine.Random.Range(crewRange.Item1 / 100, crewRange.Item2 / 100)) * 100;
        }

        return request;
    }

    private int GetRewardAmount(ShipClassification classification)
    {
        int baseStatsPrice = shipBaseStatsList.First(baseStat => baseStat.shipClass == classification).basePrice;
        return Mathf.RoundToInt(baseStatsPrice * 1.6f);
    }

    private void LoadScriptableObjects()
    {
        thrusterSubsystems = Resources.LoadAll<Thrusters>("ScriptableObjects/Subsystems").ToList();
        armorSubsystems = Resources.LoadAll<Armor>("ScriptableObjects/Subsystems").ToList();
        shipBaseStatsList = Resources.LoadAll<ShipBaseStats>("ScriptableObjects/BaseShipStats").ToList();
        subsystemsList = Resources.LoadAll<Subsystem>("ScriptableObjects/Subsystems").ToList();
    }

    private void OnSubmission()
    {
        int totalReward = 0;

        CurrentShipStats currentShipStats = ShipManager.Instance.currentShip.GetComponent<CurrentShipStats>();

        if (currentShipStats.baseStats == null)
        {
            Debug.LogError("No design submitted!");
            return;
        }

        if (!FulfillsRequirements() || !IsShipFunctional(currentShipStats))
        {
            feedbackPanel.Show();
            totalReward -= CurrentShipStats.Instance.currentPrice;
        }
        else
        {
            numSuccesses++;
            totalReward += activeShipRequest.reward;
        }

        afterActionReportPanel.Show();

        GameManager.Instance.ModifyCredits(totalReward);

        noShipSelectedText.ShowText();
        
        activeShipRequest = GenerateNewRequest();
        SetRequestText();
        
        ShipManager.Instance.ClearShip();
        
        requestNumber++;
    }

    private bool FulfillsRequirements()
    {
        CurrentShipStats ship = ShipManager.Instance.currentShip.GetComponent<CurrentShipStats>();

        bool metSublightSpeedReq = ship.currentSublightSpeed >= activeShipRequest.minSublightSpeed;
        if (!metSublightSpeedReq && activeShipRequest.minSublightSpeed > 0)
            feedbackPanel.AddSublightSpeedDiscrepancy(ship, activeShipRequest);
        
        bool metSpeedReq = ship.currentSpeed >= activeShipRequest.minSpeed;
        if (!metSpeedReq)
            feedbackPanel.AddSpeedDiscrepancy(ship, activeShipRequest);

        bool metClassReq = activeShipRequest.shipClass.Equals(ship.baseStats.shipClass);
        if (!metClassReq)
            feedbackPanel.AddShipClassDiscrapancy(ship, activeShipRequest);
        
        bool metFtlReq = true;
        if (activeShipRequest.isFtlCapable && !ship.subsystems.Any(subsystem => subsystem is FTLDrive))
        {
            feedbackPanel.AddFtlDiscrepancy(ship, activeShipRequest);
            metFtlReq = false;
        }

        bool metAtmosphereReq = true;
        if (activeShipRequest.isAtmosphereCapable && !armorSubsystems.Any(armor => ((Armor)armor).canEnterAtmosphere))
        {
            feedbackPanel.AddAtmosphereDiscrepancy(ship, activeShipRequest);
            metAtmosphereReq = false;
        }

        bool metAutonomousReq = true;
        if (activeShipRequest.isAutonomous && !ship.subsystems.Any(subsystem => subsystem is ArtificialIntelligence))
        {
            feedbackPanel.AddAiDiscrepancy(ship, activeShipRequest);
            metAutonomousReq = false;
        }

        bool metArmorReq = ship.currentArmorRating >= activeShipRequest.minArmorRating;
        if (!metArmorReq)
            feedbackPanel.AddArmorDiscrepancy(ship, activeShipRequest);

        bool shipHasAdequatePower = ship.currentPowerDraw <= ship.currentMaxPower;
        if (!shipHasAdequatePower)
            feedbackPanel.AddPowerDiscrepancy(ship);

        bool metCrewReq = ship.currentCrew >= activeShipRequest.minCrew;
        if (!metCrewReq)
            feedbackPanel.AddCrewDiscrepancy(ship, activeShipRequest);

        bool metShieldReq = ship.currentShielding >= activeShipRequest.minShieldStrength;
        if (!metShieldReq)
            feedbackPanel.AddShieldDiscrepancy(ship, activeShipRequest);

        return metSpeedReq && metClassReq && metFtlReq && metAtmosphereReq && metAutonomousReq && shipHasAdequatePower && metArmorReq && metCrewReq;
    }

    private void SetRequestText()
    {
        headerText.text = $"Request #{requestNumber:D2}";

        requestText.text = BuildRequestString();
    }

    private string BuildRequestString()
    {
        KeyValuePair<string, List<string>> customerOrg = orgNames.ElementAt(UnityEngine.Random.Range(0, orgNames.Count));
        string customerName = customerOrg.Value[UnityEngine.Random.Range(0, customerOrg.Value.Count)];
        string header = $"Name: <b>{customerName}</b>\n" +
                        $"Organization: <b>{customerOrg.Key}</b>\n\n";

        List<string> requirements = new List<string>();

        requirements.Add($"a minimum speed of {activeShipRequest.minSpeed} m/s");

        if (activeShipRequest.isAtmosphereCapable)
            requirements.Add("the ability to enter and exit a planet's atmosphere");

        if (activeShipRequest.isFtlCapable)
            requirements.Add("a faster-than-light drive");

        if (activeShipRequest.isAutonomous)
            requirements.Add("onboard artificial intelligence");

        if (activeShipRequest.minShieldStrength > 0)
            requirements.Add($"a total shield strength of at least {activeShipRequest.minShieldStrength}");

        if (activeShipRequest.minArmorRating > 0)
            requirements.Add($"an armor rating of at least {Utilities.ArmorRatingToString(activeShipRequest.minArmorRating)}");

        if (activeShipRequest.minCrew > 0)
            requirements.Add($"supports a crew of at least {activeShipRequest.minCrew}");

        string requirementsString;
        if (requirements.Count == 1)
            requirementsString = requirements[0];
        else if (requirements.Count == 2)
            requirementsString = $"{requirements[0]} and {requirements[1]}";
        else
            requirementsString = string.Join(", ", requirements.Take(requirements.Count - 1)) + ", and " + requirements.Last();

        string classString = activeShipRequest.shipClass.ToString().ToLower();
        string body = $"{customerName} wants a {classString} that has {requirementsString}.\n\n" +
            $"The reward for this contract is <b>₡{activeShipRequest.reward:N0}</b>.\n\n";

        if (activeShipRequest.budget == 0)
            body += "There is <b>no budget</b> for this contract.";
        else
            body += $"There is a budget of <b>₡{activeShipRequest.budget}</b> for this contract.";

        return header + body;
    }
}
