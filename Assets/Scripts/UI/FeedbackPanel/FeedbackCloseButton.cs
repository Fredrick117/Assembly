using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeedbackCloseButton : MonoBehaviour
{
    [SerializeField]
    public GameObject feedbackPanel;

    public void OnClosePressed()
    {
        feedbackPanel.SetActive(false);
    }
}
