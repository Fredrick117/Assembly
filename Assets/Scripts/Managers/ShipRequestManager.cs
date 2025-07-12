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

    private int randomRequestNumber = 0;

    public RequestData activeShipRequest = new RequestData();

    delegate void CreateNewRequest(RequestData Request);
    CreateNewRequest createNewShipRequest;

    public TMP_Text requestText;

    public TMP_Text headerText;

    public List<ShipBaseStats> shipBaseStats = new List<ShipBaseStats>();

    public Dictionary<Species, DamageType> speciesWeaponPreferences = new Dictionary<Species, DamageType> 
    { 
        { Species.Human, DamageType.Kinetic },
        { Species.Vynotian, DamageType.Plasma },
        { Species.Arachnid, DamageType.Laser }
    };

    public Dictionary<Species, int> speciesMilitaryShipProbability = new Dictionary<Species, int>
    {
        { Species.Human, 67 },
        { Species.Vynotian, 30 },
        { Species.Arachnid, 90 },
    };

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
        randomRequestNumber = Random.Range(1, 100000);

        headerText.text = $"Request #{randomRequestNumber}";

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

    private RequestData CreateRandomShipRequest()
    {
        RequestData request = new RequestData();

        request.customerSpecies = Utilities.GetRandomEnumValue<Species>();

        int contextProbability = Random.Range(0, 100);
        ShipClass shipClass;
        if (contextProbability < speciesMilitaryShipProbability[request.customerSpecies])
        {
            shipClass = (ShipClass)Random.Range(4, 12);
        }
        else
        {
            shipClass = (ShipClass)Random.Range(0, 4);
        }

        request.shipClass = shipClass;

        
        if (request.customerSpecies == Species.Arachnid)
        {
            request.isFtlCapable = false;
        }
        else
        {
            request.isFtlCapable = Utilities.FlipCoin();
        }

        request.isAtmosphereCapable = Utilities.FlipCoin();

        if (request.customerSpecies != Species.Arachnid)
        {
            request.isAutonomous = false;
        }
        else
        {
            request.isAutonomous = Utilities.FlipCoin();
        }

        request.armorMaterial = Utilities.GetRandomEnumValue<ArmorMaterial>();

        return request;
    }

    public void SetNewRequest()
    {
        activeShipRequest = CreateRandomShipRequest();
    }

    private void OnSubmission()
    {
        if (!FulfillsRequirements())
        {
            attemptsRemaining--;
            if (attemptsRemaining <= 0)
            {
                GameManager.Instance.ShowGameOverScreen();
            }
        }
        else
        {
            numSuccesses++;
            randomRequestNumber++;

            ShipManager.Instance.ClearShip();
            //CreateRandomShipRequest();
            SetNewRequest();
            SetRequestText();
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
       // bool metArmorReq = activeShipRequest.minArmor <= ship.Armor;
        //bool metPowerReq = activeShipRequest.minPower <= ship.currentMaxPower;

        return /*metSpeedReq && */metClassReq && metUnarmedReq /*&& metArmorReq && metPowerReq*/;
    }

    private void SetRequestText()
    {
        headerText.text = $"Request #{randomRequestNumber}";

        requestText.text = $"Customer species: <b>{activeShipRequest.customerSpecies.ToString()}</b>\n";
        requestText.text += $"Ship class: <b>{activeShipRequest.shipClass.ToString()}</b>\n";
        //requestText.text += "Minimum speed: <b>" + activeShipRequest.minSpeed.ToString() + " m/s</b>\n\n";
        requestText.text += $"Armed: <b>{(activeShipRequest.isUnarmed ? "no" : "yes")}</b>\n";
        requestText.text += $"Capable of FTL travel: <b>{(activeShipRequest.isFtlCapable ? "yes" : "no")}</b>\n";
        requestText.text += $"Capable of atmospheric entry: <b>{(activeShipRequest.isAtmosphereCapable ? "yes" : "no")}</b>\n";
        requestText.text += $"Autonomous: <b>{(activeShipRequest.isAutonomous? "yes" : "no")}</b>\n";
        requestText.text += $"Requested armor material: <b>{activeShipRequest.armorMaterial}</b>";
        //requestText.text += "Has an <b>armor</b> rating of at least <b>" + activeShipRequest.minArmor + "</b>\n\n";
        //requestText.text += "Has a total power output of at least <b>" + activeShipRequest.minPower + " GW</b>\n\n";
    }
}
