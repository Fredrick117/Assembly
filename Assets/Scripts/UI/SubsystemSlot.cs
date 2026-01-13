using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SubsystemSlot : MonoBehaviour, 
                             IPointerClickHandler, 
                             IPointerEnterHandler,
                             IPointerExitHandler
{
    [HideInInspector]
    public Subsystem subsystemData;
    [HideInInspector]
    public int slotIndex = -1;

    [Header("Set these in editor")]
    public Image icon;
    public TMP_Text slotText;
    public Image background;

    [SerializeField]
    private Color highlightColor = new Color(0.1f, 0.1f, 0.1f);

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

        CurrentShipStats.Instance.RemoveSubsystem(subsystemData);

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
        GameManager.Instance.subsystemSelectionMenu.GetComponentInChildren<SubsystemSelectionMenu>().SetSelectedSlot(slotIndex);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        background.color = Color.gray;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        background.color = Color.white;
    }
}
