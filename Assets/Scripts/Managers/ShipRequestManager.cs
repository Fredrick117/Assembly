using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ShipRequestManager : MonoBehaviour
{
    private int attemptsRemaining = 3;

    public RequestData activeShipRequest;

    delegate void CreateNewRequest(RequestData Request);
    CreateNewRequest createNewShipRequest;

    public TMP_Text requestText;

    public List<BaseShipStats> shipBaseStats = new List<BaseShipStats>();

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
        SetNewRequest();
    }

    private void OnSubmission()
    {
        if (IsValidShip())
        {
            Debug.Log("Ship is valid!");
            attemptsRemaining = 0;
            SetNewRequest();
        }
        else
        {
            attemptsRemaining--;
            if (attemptsRemaining <= 0)
            {
                Debug.LogError("You lose!");
                Application.Quit();
            }
        }

        SubmitDesign();
        ShipManager.Instance.ClearShip();
        SetNewRequest();
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
        Debug.LogError("IsValidShip not yet implemented");
        return false;
    }

    public void SubmitDesign()
    {
        Debug.LogError("Not yet implemented");
    }

    private void SetRequestText()
    {
        requestText.text = "Is a <b>" + activeShipRequest.shipClass.ToString() + "</b>\n\n";
        requestText.text += "Has a speed of at least <b>" + activeShipRequest.minSpeed.ToString() + " m/s</b>\n\n";
        requestText.text += "Is " + (activeShipRequest.unarmed ? "unarmed" : "armed") + "\n\n";

        //string shipSize = "";
        //switch (activeShipRequest.size)
        //{
        //    case 1:
        //        shipSize = "small";
        //        break;
        //    case 2:
        //        shipSize = "large";
        //        break;
        //    default:
        //        shipSize = "???";
        //        break;
        //}

        //requestText.text += "Is a <b>" + shipSize + "</b> ship";
    }

    /// <summary>
    /// Creates a random request for a ship and sets it to the current ship request.
    /// </summary>
    public void SetNewRequest()
    {
        RequestData data = new RequestData();
        data.shipClass = Utilities.GetRandomEnumValue<ShipClass>().ToString();
        //data.size = Random.Range(0, 4);
        data.minSpeed = shipBaseStats[Random.Range(0, shipBaseStats.Count)].baseSpeed;
        data.unarmed = Random.Range(0, 2) == 0 ? false : true;

        activeShipRequest = data;

        SetRequestText();
    }
}
