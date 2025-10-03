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

    //public void AddArmorDiscrepancy(ShipStats submission, RequestData request)
    //{
    //    GameObject customer = GameObject.Instantiate(listItemPrefab, customerRequestColumn);
    //    customer.GetComponent<TMP_Text>().text = request.armorMaterial != ArmorMaterial.None ? 
    //        request.armorMaterial.ToString() : "No armor plating";

    //    GameObject player = GameObject.Instantiate(listItemPrefab, playerSubmissionColumn);

    //    if (!submission.subsystems.Values.Any(subsystem => subsystem is Armor) && request.armorMaterial != ArmorMaterial.None)
    //    {
    //        player.GetComponent<TMP_Text>().text = "No armor plating";
    //        return;
    //    }

    //    player.GetComponent<TMP_Text>().text = submission.ArmorMaterial.ToString();
    //}

    public void AddArmedDiscrepancy(ShipStats submission, RequestData request)
    {
        GameObject customer = GameObject.Instantiate(listItemPrefab, customerRequestColumn);
        GameObject player = GameObject.Instantiate(listItemPrefab, playerSubmissionColumn);

        customer.GetComponent<TMP_Text>().text = request.isArmed ? "Armed" : "Unarmed";

        if (submission.subsystems.Values.Any(subsystem => subsystem is Weapon))
            player.GetComponent<TMP_Text>().text = "Armed";
        else
            player.GetComponent<TMP_Text>().text = "Unarmed";
    }

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
            Thrusters thrusters = (Thrusters)submission.subsystems.Values.First(subsystem => subsystem is Thrusters);
            player.GetComponent<TMP_Text>().text = thrusters.atmosphericEntryCapable ? "Possible" : "Not possible";
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
}
