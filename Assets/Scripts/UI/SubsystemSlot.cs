using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SubsystemSlot : MonoBehaviour, IPointerClickHandler
{
    public Subsystem subsystemData;
    public int slotIndex = -1;

    // Already set in editor
    public Image icon;

    private void Start()
    {
        
    }

    public void SetSubsystem(Subsystem subsystem)
    {
        subsystemData = subsystem;
    }

    public void RemoveSubsystem()
    {
        ShipStats.Instance.RemoveSubsystem(slotIndex);

        if (subsystemData != null)
        {
            subsystemData = null;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            RemoveSubsystem();
            icon.sprite = null;
            print("Removed subsystem");
            return;
        }

        GameManager.Instance.ShowSubsystemMenu();
        GameManager.Instance.subsystemSelectionMenu.GetComponent<SubsystemSelectionMenu>().SetSelectedSlot(slotIndex);
    }
}
