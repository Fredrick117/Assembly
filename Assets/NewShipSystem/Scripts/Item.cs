using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Item : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    [HideInInspector]
    public static Item currentDraggedItem;

    private bool isDragging = false;
    private RectTransform rectTransform;
    private Canvas canvas;
    private CanvasGroup canvasGroup;

    public ItemData data;

    [HideInInspector]
    // The slot on the grid that previously held this item
    public GridSlot previousSlot;
    // The slot on the grid that this item is currently occupying
    public GridSlot currentSlot;
    // The slot on the grid that this item is currently hovering over (only valid when item is being dragged)
    private GridSlot hoveredSlot;

    private Image icon;

    public static event Action<Item> OnPickUpItem;
    public static event Action<Item> OnDropItem;

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

    private bool CanPlace(int row, int col)
    {
        if (hoveredSlot == null || hoveredSlot.parentGrid == null)
            return false;

        GridManager targetGrid = hoveredSlot.parentGrid;

        for (int r = 0; r < 3; r++)
        {
            for (int c = 0; c < 3; c++)
            {
                if (!data.IsCellFilled(r, c))
                    continue;

                int rowOffset = (r + row) - 1;
                int colOffset = (c + col) - 1;

                if (!targetGrid.IsInBounds(rowOffset, colOffset))
                    return false;

                GridSlot slot = targetGrid.gridSlots[rowOffset, colOffset];

                if (slot.isOccupied)
                    return false;
            }
        }

        return true;
    }

    private void PlaceOnGrid(int row, int col, GridManager targetGrid)
    {
        transform.position = targetGrid.GetSlot(row, col).transform.position;

        for (int r = 0; r < 3; r++)
        {
            for (int c = 0; c < 3; c++)
            {
                if (!data.IsCellFilled(r, c))
                    continue;

                int rowOffset = (r + row) - 1;
                int colOffset = (c + col) - 1;

                if (!targetGrid.IsInBounds(rowOffset, colOffset))
                    continue;

                GridSlot slot = targetGrid.gridSlots[rowOffset, colOffset];

                slot.isOccupied = true;
                slot.currentItem = this;
                slot.image.color = GridSlot.defaultColor;
            }
        }

        currentSlot = targetGrid.gridSlots[row, col];
    }

    private void PickUpFromGrid(int row, int col)
    {
        if (previousSlot == null || previousSlot.parentGrid == null)
        {
            Debug.LogError("PickUpFromGrid: previousSlot or parentGrid was null!");
            return;
        }

        GridManager targetGrid = previousSlot.parentGrid;

        for (int r = 0; r < 3; r++)
        {
            for (int c = 0; c < 3; c++)
            {
                if (!data.IsCellFilled(r, c))
                    continue;

                int rowOffset = (r + row) - 1;
                int colOffset = (c + col) - 1;

                if (!targetGrid.IsInBounds(rowOffset, colOffset))
                    continue;

                GridSlot slot = targetGrid.gridSlots[rowOffset, colOffset];

                slot.isOccupied = false;
                slot.currentItem = null;
                slot.image.color = GridSlot.defaultColor;
            }
        }
    }

    private void ReturnToPreviousPosition()
    {
        if (previousSlot == null)
        {
            Debug.LogError("No previous slot! Returning to 0,0");
            transform.position = Vector2.zero;
            return;
        }

        PlaceOnGrid(previousSlot.row, previousSlot.col, previousSlot.parentGrid);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        print("pick up!");
        
        SetAllItemsBlockRaycasts(false);
        isDragging = true;
        Item.currentDraggedItem = this;

        previousSlot = currentSlot;
        currentSlot = null;

        if (previousSlot == null)
        {
            Debug.LogError("OnPointerDown: previous slot was null!");
            return;
        }

        OnPickUpItem?.Invoke(this);
        
        PickUpFromGrid(previousSlot.row, previousSlot.col);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        print("place!");
        OnDropItem?.Invoke(null);

        SetAllItemsBlockRaycasts(true);
        isDragging = false;
        Item.currentDraggedItem = null;

        GridManager targetGrid = hoveredSlot.parentGrid;
        if (targetGrid != null)
        {
            targetGrid.ClearPlacementPreview();
        }

        if (hoveredSlot == null)
        {
            Debug.LogWarning("hoveredSlot was null!");
            ReturnToPreviousPosition();

            return;
        }

        if (!CanPlace(hoveredSlot.row, hoveredSlot.col))
        {
            Debug.LogWarning("Can't place!");
            ReturnToPreviousPosition();

            return;
        }

        PlaceOnGrid(hoveredSlot.row, hoveredSlot.col, hoveredSlot.parentGrid);
    }
}
