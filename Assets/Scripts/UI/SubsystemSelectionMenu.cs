using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SubsystemSelectionMenu : MonoBehaviour
{
    [HideInInspector]
    private int selectedSlot;

    public GameObject subsystemButtonPrefab;

    public Transform contentContainer;

    private void Start()
    {
        List<Subsystem> allSubsystems = Resources.LoadAll<Subsystem>("ScriptableObjects/Subsystems").ToList();
        
        foreach (Subsystem s in allSubsystems)
        {
            GameObject button = GameObject.Instantiate(subsystemButtonPrefab, contentContainer);
            button.GetComponent<SubsystemButton>().subsystemData = s;
        }
    }

    public int GetSelectedSlot()
    {
        return selectedSlot;
    }

    public void SetSelectedSlot(int index)
    {
        selectedSlot = index;
    }

    public void Show()
    {
        transform.parent.gameObject.SetActive(true);
    }

    public void Hide()
    {
        transform.parent.gameObject.SetActive(false);
        selectedSlot = -1;
    }
}
