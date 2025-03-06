using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SubsystemToggleButton : MonoBehaviour
{
    public TMP_Text toggleText;
    public GameObject subsystemContainer;

    public void OnToggleClicked()
    {
        if (subsystemContainer.activeSelf)
        {
            subsystemContainer.SetActive(false);
            toggleText.text = "/\\";
        }
        else
        {
            subsystemContainer.SetActive(true);
            toggleText.text = "\\/";
        }
    }
}
