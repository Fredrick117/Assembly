using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class FeedbackPanel : MonoBehaviour
{
    public GameObject listItemPrefab;

    public Transform customerRequestColumn;

    public Transform playerSubmissionColumn;

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void AddSpeedDiscrepancy(ShipStats submission, RequestData request)
    {
        GameObject customer = GameObject.Instantiate(listItemPrefab, customerRequestColumn);
        GameObject player = GameObject.Instantiate(listItemPrefab, playerSubmissionColumn);

        customer.GetComponent<TMP_Text>().text = $"Capable of {request.minSpeed} m/s";
        player.GetComponent<TMP_Text>().text = $"{submission.currentSpeed} m/s";
    }

    public void AddShipClassDiscrapancy(ShipStats submission, RequestData request)
    {
        GameObject customer = GameObject.Instantiate(listItemPrefab, customerRequestColumn);
        GameObject player = GameObject.Instantiate(listItemPrefab, playerSubmissionColumn);

        customer.GetComponent<TMP_Text>().text = request.shipClass.ToString();
        player.GetComponent<TMP_Text>().text = submission.currentClass.ToString();
    }

    public void AddArmorDiscrepancy(ShipStats submission, RequestData request)
    {
        GameObject customer = GameObject.Instantiate(listItemPrefab, customerRequestColumn);
        GameObject player = GameObject.Instantiate(listItemPrefab, playerSubmissionColumn);

        customer.GetComponent<TMP_Text>().text = Utilities.ArmorRatingToString(request.minArmorRating);
        player.GetComponent<TMP_Text>().text = Utilities.ArmorRatingToString(submission.currentArmorRating);
    }

    //public void AddArmedDiscrepancy(ShipStats submission, RequestData request)
    //{
    //    GameObject customer = GameObject.Instantiate(listItemPrefab, customerRequestColumn);
    //    GameObject player = GameObject.Instantiate(listItemPrefab, playerSubmissionColumn);

    //    customer.GetComponent<TMP_Text>().text = request.isArmed ? "Armed" : "Unarmed";

    //    if (submission.subsystems.Values.Any(subsystem => subsystem is Weapon))
    //        player.GetComponent<TMP_Text>().text = "Armed";
    //    else
    //        player.GetComponent<TMP_Text>().text = "Unarmed";
    //}

    public void AddFtlDiscrepancy(ShipStats submission, RequestData request)
    {
        GameObject customer = GameObject.Instantiate(listItemPrefab, customerRequestColumn);
        GameObject player = GameObject.Instantiate(listItemPrefab, playerSubmissionColumn);

        customer.GetComponent<TMP_Text>().text = request.isFtlCapable ? "Has FTL drive" : "No FTL drive";

        if (submission.subsystems.Values.Any(subsystem => subsystem is FTLDrive))
            player.GetComponent<TMP_Text>().text = "Has FTL drive";
        else
            player.GetComponent<TMP_Text>().text = "Has no FTL drive";
    }

    public void AddAtmosphereDiscrepancy(ShipStats submission, RequestData request)
    {
        GameObject customer = GameObject.Instantiate(listItemPrefab, customerRequestColumn);
        GameObject player = GameObject.Instantiate(listItemPrefab, playerSubmissionColumn);

        customer.GetComponent<TMP_Text>().text = request.isAtmosphereCapable ? "Atmospheric entry possible" : "Atmospheric entry not possible";

        if (submission.subsystems.Values.Any(subsystem => subsystem is Thrusters))
        {
            Armor armor = (Armor)submission.subsystems.Values.First(subsystem => subsystem is Armor);
            player.GetComponent<TMP_Text>().text = armor.canEnterAtmosphere? "Possible" : "Not possible";
        }
        else
            player.GetComponent<TMP_Text>().text = "No thrusters added";
    }

    public void AddAiDiscrepancy(ShipStats submission, RequestData request)
    {
        GameObject customer = GameObject.Instantiate(listItemPrefab, customerRequestColumn);
        GameObject player = GameObject.Instantiate(listItemPrefab, playerSubmissionColumn);

        customer.GetComponent<TMP_Text>().text = request.isAutonomous ? "Autonomous" : "Not autonomous";

        player.GetComponent<TMP_Text>().text =
                submission.subsystems.Values.Any(subsystem => subsystem is ArtificialIntelligence) ?
                "Autonomous" : "Not autonomous";
    }

    public void AddPowerDiscrepancy(ShipStats submission)
    {
        GameObject customer = GameObject.Instantiate(listItemPrefab, customerRequestColumn);
        GameObject player = GameObject.Instantiate(listItemPrefab, playerSubmissionColumn);

        customer.GetComponent<TMP_Text>().text = "";

        player.GetComponent<TMP_Text>().text = $"Power draw is {submission.currentPowerDraw} MW while power output is {submission.currentMaxPower} MW";
    }

    public void AddCrewDiscrepancy(ShipStats submission, RequestData request)
    {
        GameObject customer = GameObject.Instantiate(listItemPrefab, customerRequestColumn);
        GameObject player = GameObject.Instantiate(listItemPrefab, playerSubmissionColumn);

        customer.GetComponent<TMP_Text>().text = $"Minimum crew of {request.minCrew}";
        player.GetComponent<TMP_Text>().text = $"{submission.currentCrew}";
    }

    public void AddShieldDiscrepancy(ShipStats submission, RequestData request)
    {
        GameObject customer = GameObject.Instantiate(listItemPrefab, customerRequestColumn);
        GameObject player = GameObject.Instantiate(listItemPrefab, playerSubmissionColumn);

        customer.GetComponent<TMP_Text>().text = $"Shield rating of at least {request.minShieldStrength}";
        player.GetComponent<TMP_Text>().text = $"Shield rating was {submission.currentShielding}";
    }
}
