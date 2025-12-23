using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DebugPanel : MonoBehaviour
{
    [SerializeField] private TMP_Text currentDraggedItemText;
    [SerializeField] private TMP_Text currentItemLastLocText;
    [SerializeField] private TMP_Text hoveredGridSlotText;
    [SerializeField] private TMP_Text canPlaceCurrentItemText;

    private void SetCurrentDraggedItemText(Item item)
    {
        currentDraggedItemText.text = $"Current dragged item: {(item != null ? item.GetRuntimeData().name : "null")}";
    }

    private void SetCurrentItemLastLocText(Item item)
    {
        string lastLoc = "null";

        if (item != null && item.previousSlot != null)
            lastLoc = $"({item.previousSlot.row}, {item.previousSlot.col})";

        currentItemLastLocText.text = $"Current item last loc: {lastLoc}";
    }

    private void SetHoveredSlotText(GridSlot slot)
    {
        string slotLoc = "null";

        if (slot != null)
            slotLoc = $"({slot.row}, {slot.col})";

        hoveredGridSlotText.text = $"Hovered grid slot: {slotLoc}";
    }

    private void OnEnable()
    {
        Item.OnPickUpItem += SetCurrentDraggedItemText;
        Item.OnPickUpItem += SetCurrentItemLastLocText;

        Item.OnDropItem += SetCurrentDraggedItemText;
        Item.OnDropItem += SetCurrentItemLastLocText;

        GridSlot.OnSlotEnter += SetHoveredSlotText;
        GridSlot.OnSlotExit += SetHoveredSlotText;
    }

    private void OnDisable() 
    {
        Item.OnPickUpItem -= SetCurrentDraggedItemText;
        Item.OnPickUpItem -= SetCurrentItemLastLocText;

        Item.OnDropItem -= SetCurrentDraggedItemText;
        Item.OnDropItem -= SetCurrentItemLastLocText;

        GridSlot.OnSlotEnter -= SetHoveredSlotText;
        GridSlot.OnSlotExit -= SetHoveredSlotText;
    }
}
