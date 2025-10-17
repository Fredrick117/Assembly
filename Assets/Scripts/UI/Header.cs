using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Header : MonoBehaviour
{
    [SerializeField]
    private TMP_Text fundsText;
    [SerializeField]
    private TMP_Text marketShareText;
    [SerializeField]
    private TMP_Text contractsText;

    private void OnEnable()
    {
        EventManager.onSubmit += UpdateHeaderText;
    }

    private void OnDisable()
    {
        EventManager.onSubmit -= UpdateHeaderText;
    }

    public void UpdateHeaderText()
    {
        string formattedFunds = GameManager.Instance.currentCredits.ToString("N0");
        fundsText.text = $"Funds remaining: ₡{formattedFunds}";
        //marketShareText.text = $"Failures remaining: {ShipRequestManager.Instance.attemptsRemaining}";
    }
}
