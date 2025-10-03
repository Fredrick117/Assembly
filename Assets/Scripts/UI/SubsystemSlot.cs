using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SubsystemSlot : MonoBehaviour, IPointerClickHandler
{
    public Subsystem subsystemData;
    public int slotIndex = -1;

    // Already set in editor
    public Image icon;

    public TMP_Text slotText;

    public void SetSubsystem(Subsystem subsystem)
    {
        subsystemData = subsystem;
    }

    public void RemoveSubsystem()
    {
        print(subsystemData);
        if (subsystemData == null)
        {
            Debug.LogWarning("No subsystem data to remove!");
        }

        ShipStats.Instance.RemoveSubsystem(slotIndex);

        if (subsystemData != null)
        {
            subsystemData = null;
        }

        icon.sprite = null;
        slotText.text = "";
        print("Removed subsystem");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            RemoveSubsystem();
            return;
        }

        GameManager.Instance.ShowSubsystemMenu();
        GameManager.Instance.subsystemSelectionMenu.GetComponent<SubsystemSelectionMenu>().SetSelectedSlot(slotIndex);
    }
}
