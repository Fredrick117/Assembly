using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SubsystemSlot : MonoBehaviour, IPointerClickHandler
{
    public GameObject subsystemSelectionMenu;
    public Subsystem subsystem;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!subsystemSelectionMenu.activeSelf)
        {
            subsystemSelectionMenu.SetActive(true);
        }

        subsystemSelectionMenu.GetComponent<SubsystemSelectionMenu>().selectedSlot = this;
    }
}
