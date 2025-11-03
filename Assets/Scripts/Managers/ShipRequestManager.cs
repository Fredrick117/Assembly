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

    private NoShipSelectedText noShipSelectedText;

    private static List<ShipBaseStats> shipBaseStatsList = new List<ShipBaseStats>();

    private static List<Subsystem> subsystemsList = new List<Subsystem>();

    private static List<Thrusters> thrusterSubsystems = new();
    private static List<Armor> armorSubsystems = new();

    Dictionary<string, List<string>> orgNames = new()
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
                "Varrash",
                "Torkal'Neth",
                "Zhur'ka",
                "Maath",
                "Khaal'tek",
                "Ornak",
                "Shra'ven",
                "Kaarn",
                "Drek'ra",
                "Vorr'uun",
                "Thresh",
                "Graal'Tekh",
                "Nur'vak",
                "Shuun",
                "Vorrek",
                "Xel'nor",
                "Korr'vek",
                "Tar'vus",
                "Baal'drun",
                "Kaath'ra"
            }
        }
    };

    Dictionary<ShipClassification, Tuple<int, int>> shipSpeedRanges = new()
    {
        { ShipClassification.Corvette, new Tuple<int, int>(300, 500) },
        { ShipClassification.Destroyer, new Tuple<int, int>(200, 400) },
        { ShipClassification.Cruiser, new Tuple<int, int>(100, 300) },
        { ShipClassification.Carrier, new Tuple<int, int>(100, 300) },
    };

    Dictionary<ShipClassification, List<ShipRole>> shipRoles = new()
    {
        { ShipClassification.Corvette, new List<ShipRole> { ShipRole.Ship_To_Ship, ShipRole.Recon, ShipRole.Escort, ShipRole.Patrol, ShipRole.Enforcement } },
        { ShipClassification.Destroyer, new List<ShipRole> { ShipRole.Ship_To_Ship, ShipRole.Recon, ShipRole.Escort, ShipRole.Enforcement } },
        { ShipClassification.Cruiser, new List<ShipRole> { ShipRole.Ship_To_Ship, ShipRole.Enforcement, ShipRole.Escort } },
        { ShipClassification.Carrier, new List<ShipRole> { ShipRole.Carrier, ShipRole.Transport } }
    };

    Dictionary<ShipClassification, Tuple<int, int>> shipCrewRanges = new()
    {
        { ShipClassification.Corvette, new Tuple<int, int>(50, 250) },
        { ShipClassification.Destroyer, new Tuple<int, int>(50, 350) },
        { ShipClassification.Cruiser, new Tuple<int, int>(100, 450) },
        { ShipClassification.Carrier, new Tuple<int, int>(200, 1000) },
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
        thrusterSubsystems = Resources.LoadAll<Thrusters>("ScriptableObjects/Subsystems").ToList();
        armorSubsystems = Resources.LoadAll<Armor>("ScriptableObjects/Subsystems").ToList();
        shipBaseStatsList = Resources.LoadAll<ShipBaseStats>("ScriptableObjects/Ships").ToList();

        noShipSelectedText = mainCanvas.GetComponentInChildren<NoShipSelectedText>();
        
        LoadScriptableObjects();

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

    public RequestData GenerateNewRequest()
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
            request.isAtmosphereCapable = atmosphereCapable;
        }

        bool ftlCapable = Utilities.FlipCoin();
        if (ftlCapable)
        {
            request.reward += 100000;
            request.isFtlCapable = ftlCapable;
        }

        request.minArmorRating = (int)Utilities.GetRandomEnumValue<ArmorRating>(true);
        request.reward += 1000000;

        bool aiRequired = Utilities.FlipCoin();
        if (aiRequired)
        {
            request.reward += 500000;
            request.isAutonomous = aiRequired;
        }

        if (!request.isAutonomous)
        {
            Tuple<int, int> crewRange = shipCrewRanges[request.shipClass];
            request.minCrew = Mathf.RoundToInt(UnityEngine.Random.Range(crewRange.Item1 / 100, crewRange.Item2 / 100)) * 100;
        }

        return request;
    }

    //private RequestData GetRequestDataFromShip(Ship ship)
    //{
    //    RequestData request = new();

    //    Armor armor = (Armor)ship.subsystems.FirstOrDefault(a => a is Armor);
    //    if (armor != null)
    //    {
    //        request.isAtmosphereCapable = armor.canEnterAtmosphere;
    //    }
    //    else
    //    {
    //        Debug.LogError("ShipRequestManager: no thrusters :(");
    //    }

    //    request.minSpeed = thrusterSubsystems[UnityEngine.Random.Range(0, thrusterSubsystems.Count())].speed;
    //    request.isFtlCapable = ship.isFTL;
    //    request.isAutonomous = ship.isAutonomous;
    //    request.isAtmosphereCapable = ship.isAtmospheric;
    //    request.shipClass = ship.classification;
    //    request.minArmorRating = ship.armorRating;

    //    Shielding shields = (Shielding)ship.subsystems.FirstOrDefault(s => s is Shielding);
    //    if (shields != null)
    //    {
    //        request.minShieldStrength = shields.shieldStrength;
    //    }

    //    request.reward = GetRewardAmount(request.shipClass);

    //    return request;
    //}

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
            feedbackPanel.Show();
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
        activeShipRequest = GenerateNewRequest();
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
        
        bool metFtlReq = true;
        if (activeShipRequest.isFtlCapable && !ship.subsystems.Values.Any(subsystem => subsystem is FTLDrive))
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
        if (activeShipRequest.isAutonomous && !ship.subsystems.Values.Any(subsystem => subsystem is ArtificialIntelligence))
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
        headerText.text = $"Request #{requestNumber.ToString("D2")}";

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
        string body = $"{customerName} wants a {classString}-class ship that has {requirementsString}.\n\n" +
            $"The reward for this contract is <b>₡{activeShipRequest.reward.ToString("N0")}</b>.\n\n";

        if (activeShipRequest.budget == 0)
            body += "There is <b>no budget</b> for this contract.";
        else
            body += $"There is a budget of <b>₡{activeShipRequest.budget}</b> for this contract.";

        return header + body;
    }
}
