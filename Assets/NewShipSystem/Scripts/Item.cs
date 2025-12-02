using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Item : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [HideInInspector]
    public static Item currentDraggedItem;

    private bool isDragging = false;
    private RectTransform rectTransform;
    private Canvas canvas;
    private CanvasGroup canvasGroup;

    public ItemData data;

    [HideInInspector]
    public GridSlot currentSlot;
    private GridSlot hoveredSlot;

    private Image icon;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
        icon = GetComponentInChildren<Image>();
    }

    void Start()
    {
        icon.sprite = data.itemSprite;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isDragging)
        {
            transform.position = eventData.position;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        SetAllItemsBlockRaycasts(false);

        isDragging = true;
        Item.currentDraggedItem = this;

        if (hoveredSlot != null)
            hoveredSlot.isOccupied = false;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        SetAllItemsBlockRaycasts(true);

        canvasGroup.blocksRaycasts = true;
        isDragging = false;
        Item.currentDraggedItem = null;

        if (hoveredSlot != null && !hoveredSlot.isOccupied)
        {
            transform.position = hoveredSlot.transform.position;
            hoveredSlot.image.color = GridSlot.defaultColor;
            //hoveredSlot.isOccupied = true;
        }

        GridManager.Instance.ClearPlacementPreview();
    }

    public void SetHoveredSlot(GridSlot slot)
    {
        hoveredSlot = slot;
    }

    private static void SetAllItemsBlockRaycasts(bool blocks)
    {
        foreach (var item in FindObjectsOfType<Item>())
        {
            if (item == null || item.canvasGroup == null)
            {
                continue;
            }

            item.canvasGroup.blocksRaycasts = blocks;
        }
    }

    private void PlaceOnGrid(int row, int col)
    {
        for (int r = 0; r < 3; r++)
        {
            for (int c = 0; c < 3; c++)
            {
                if (!data.IsCellFilled(r, c))
                    continue;

                GridSlot slot = GridManager.Instance.gridSlots[(r + row) - 1, (c + col) - 1];
                slot.isOccupied = true;
                slot.currentItem = this;
                slot.image.color = GridSlot.defaultColor;
            }
        }
    }

    private void PickUpFromGrid(int row, int col)
    {
        for (int r = 0; r < 3; r++)
        {
            for (int c = 0; c < 3; c++)
            {
                GridSlot slot = GridManager.Instance.gridSlots[(r + row) - 1, (c + col) - 1];
                slot.isOccupied = false;
                slot.currentItem = null;
                slot.image.color = GridSlot.defaultColor;
            }
        }
    }
}
