using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Header : MonoBehaviour
{
    [SerializeField]
    private TMP_Text submissionsText;
    [SerializeField]
    private TMP_Text attemptsText;

    private void Start()
    {
        SetSubmissionStats();
    }

    private void OnEnable()
    {
        EventManager.onSubmit += SetSubmissionStats;
    }

    private void OnDisable()
    {
        EventManager.onSubmit -= SetSubmissionStats;
    }

    private void SetSubmissionStats()
    {
        submissionsText.text = $"Successful submissions: {ShipRequestManager.Instance.numSuccesses}";
        attemptsText.text = $"Failures remaining: {ShipRequestManager.Instance.attemptsRemaining}";
    }
}
