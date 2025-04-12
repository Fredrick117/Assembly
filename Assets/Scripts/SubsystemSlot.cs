using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SubsystemSlot : MonoBehaviour, IPointerClickHandler
{
    private Subsystem subsystem;
    public GameObject subsystemMenu;

    private Image icon;

    private void Awake()
    {
        //icon = transform.GetChild(0).GetComponent<Image>();
    }

    public Subsystem GetSubsystem()
    {
        return subsystem;
    }

    public void SetSubsystem(Subsystem _subsystem)
    {
        subsystem = _subsystem;

        icon.sprite = LoadSpriteFromPath(_subsystem.icon);
    }

    // Helper method to load sprite from path
    private Sprite LoadSpriteFromPath(string path)
    {
        // Option 1: If your sprites are in Resources folder
        if (!string.IsNullOrEmpty(path))
        {
            string resourcePath = path.Replace(".jpg", "").Replace(".png", "");
            return Resources.Load<Sprite>(resourcePath);
        }

        return null;
    }

    public void RemoveSubsystem()
    {
        ShipManager.Instance.RemoveSubsystem(subsystem);

        if (subsystem != null)
        {
            subsystem = null;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            RemoveSubsystem();
            return;
        }

        if (!subsystemMenu.activeSelf)
        {
            subsystemMenu.SetActive(true);
            subsystemMenu.GetComponent<SubsystemSelectionMenu>().SetSelectedSlot(this);
        }
    }
}
