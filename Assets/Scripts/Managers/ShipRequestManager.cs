using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShipRequestManager : MonoBehaviour
{
    public ShipRequestData currentShipRequest;

    public ShipContract selectedContract;

    delegate void CreateNewRequest(ShipRequestData Request);
    CreateNewRequest createNewShipRequest;

    public TMP_Text requestText;

    //public TextAsset requestData;

    private void OnEnable()
    {
        EventManager.onSubmit += OnSubmission;
    }

    private void OnDisable()
    {
        EventManager.onSubmit -= OnSubmission;
    }

    // Start is called before the first frame update
    void Start()
    {
        //SetNewRequest();
        //SetRequestText();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnSubmission()
    {
        print("Checking if ship is valid...");
        print("Ship is valid: " + IsValidShip());
    }

    private void AcceptContract()
    {

    }

    private void RemoveContractFromList()
    {
        foreach (Transform child in transform)
        {
            
        }
    }

    /// <summary>
    /// Checks if the current design is a valid ship design.
    /// A valid ship design:
    ///     - Does not have any exposed segments
    ///     - Has an engine/thrusters
    ///     - Has at least two modules
    /// </summary>
    /// <returns>Whether or not the design is valid</returns>
    private bool IsValidShip()
    {
        GameObject root = ShipManager.Instance.rootModule;

        if (root == null)
        {
            return false;
        }

        foreach (Connector connector in root.GetComponent<ShipModule>().connectors)
        {
            if (!connector.otherConnector)
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Check to see if the current design satisfies the customer's request
    /// </summary>
    public void CheckAgainstRequest()
    {
        
    }

    public void SetRequestText()
    {
        requestText.text = "Current Request:\n" +
                           "Ship Type: " + currentShipRequest.shipType.ToString() + "\n" + 
                           "Ship Class: " + currentShipRequest.shipClass.ToString() + "\n" +
                           "Mininum Speed: " + currentShipRequest.minSpeed.ToString() + "\n" +
                           "Maximum Speed: " + currentShipRequest.maxSpeed.ToString() + "\n\n";

        requestText.text += "Required Subsystems:\n";

        foreach (Subsystem subsystem in currentShipRequest.requiredSubsystems)
        {
            requestText.text += "\t" + subsystem.ToString() + "\n";
        }

        requestText.text += "\nBudget: " + currentShipRequest.budget.ToString();
    }

    /// <summary>
    /// Creates a random request for a ship and sets it to the current ship request.
    /// </summary>
    public void SetNewRequest()
    {
        // TODO: get data from JSON file so that random distribution isn't equal
        
        ShipRequestData data = new ShipRequestData();
        data.shipType = Utilities.GetRandomEnumValue<ShipType>();
        data.shipClass = Utilities.GetRandomEnumValue<ShipClass>();

        switch (data.shipClass)
        {
            case ShipClass.Corvette:
                data.budget = Random.Range(10000, 50000);
                data.minSpeed = 5;
                data.maxSpeed = 10;
                break;
            case ShipClass.Destroyer:
                data.budget = Random.Range(40000, 75000);
                data.minSpeed = 4;
                data.maxSpeed = 8;
                break;
            case ShipClass.Carrier:
                data.budget = Random.Range(100000, 200000);
                data.minSpeed = 1;
                data.maxSpeed = 3;
                break;
            default:
                data.budget = 0;
                data.minSpeed = 0;
                data.maxSpeed = 0;
                break;
        }

        data.requiredSubsystems = new HashSet<Subsystem>();
        
        for (int i = 0; i < Random.Range(1, System.Enum.GetNames(typeof(Subsystem)).Length); i++)
        {
            //data.requiredSubsystems.Add(Utilities.GetRandomEnumValue<Subsystem>());
        }

        currentShipRequest = data;
    }
}
