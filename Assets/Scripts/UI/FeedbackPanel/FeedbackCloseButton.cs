using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeedbackCloseButton : MonoBehaviour
{
    [SerializeField]
    public FeedbackPanel feedbackPanel;

    // TODO: move the contents of this function to feedbackPanel
    public void OnClosePressed()
    {
        feedbackPanel.gameObject.SetActive(false);

        foreach (Transform child in feedbackPanel.playerSubmissionColumn.transform)
        {
            if (!child.gameObject.CompareTag("FeedbackHeader"))
                Destroy(child.gameObject);
        }

        foreach (Transform child in feedbackPanel.customerRequestColumn.transform)
        {
            if (!child.gameObject.CompareTag("FeedbackHeader"))
                Destroy(child.gameObject);
        }
    }
}
