using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AfterActionReportPanel : MonoBehaviour
{
    [SerializeField] private TMP_Text headerText;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private GridManager subsystemGrid;

    private string noThrusterText =
        "Because the ship you provided had no thrusters, the customer refused to pay us and is demanding a refund.";

    public void Show()
    {
        gameObject.SetActive(true);
        
        SetReportText();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        subsystemGrid.ClearGrid();
    }

    private void SetReportText()
    {
        CurrentShipStats currentShipStats = CurrentShipStats.Instance;
        
        // If ship doesn't have thrusters, print a message saying the customer refused to pay because the ship wouldn't move
        if (!currentShipStats.HasThrusters())
        {
            descriptionText.text = noThrusterText;
        }
    }
}
