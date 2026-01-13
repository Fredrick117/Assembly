using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Item : MonoBehaviour, IPointerClickHandler
{
    public static Item currentDraggedItem;
    
    private bool isDragging = false;
    private CanvasGroup canvasGroup;

    public ItemData data;
    public Subsystem subsystem;

    [HideInInspector]
    // The slot on the grid that previously held this item
    public GridSlot previousSlot;
    // The slot on the grid that this item is currently occupying
    public GridSlot currentSlot;
    // The slot on the grid that this item is currently hovering over (only valid when item is being dragged)
    private GridSlot hoveredSlot;

    private Image icon;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        icon = GetComponentInChildren<Image>();
    }

    private void Update()
    {
        if (isDragging)
        {
            transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
            
            if (Input.GetMouseButtonDown(0))
            {
                Place();
            }
        }
    }

    public void SetSprite(Sprite sprite)
    {
        float spriteWidthScale = sprite.rect.width / 32f;
        float spriteHeightScale = sprite.rect.height / 32f;

        GetComponent<RectTransform>().sizeDelta = new Vector2(50f * spriteWidthScale, 50f * spriteHeightScale);
        
        icon.sprite = sprite;
    }

    public void SetSubsystemData(Subsystem subsystemData)
    {
        subsystem = subsystemData;
        SetSprite(subsystemData.icon);
    }

    public void SetHoveredSlot(GridSlot slot)
    {
        hoveredSlot = slot;
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

    private void PlaceOnGrid(int row, int col)
    {
		GridManager grid = GridManager.Instance;
        transform.position = grid.GetSlot(row, col).transform.position;

        for (int r = 0; r < 3; r++)
        {
            for (int c = 0; c < 3; c++)
            {
                if (!data.IsCellFilled(r, c))
                    continue;

                int rowOffset = (r + row) - 1;
                int colOffset = (c + col) - 1;

                if (!grid.IsInBounds(rowOffset, colOffset))
                    continue;

                GridSlot slot = GridManager.Instance.gridSlots[rowOffset, colOffset];

                slot.isOccupied = true;
                slot.currentItem = this;
                slot.image.color = GridSlot.defaultColor;
            }
        }

        currentSlot = grid.gridSlots[row, col];
    }

    private void RemoveFromGrid(int row, int col)
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

        PlaceOnGrid(previousSlot.row, previousSlot.col);
    }
    
    public void PickUp()
    {
        SetAllItemsBlockRaycasts(false);
        isDragging = true;
        currentDraggedItem = this;
        
        previousSlot = currentSlot;
        currentSlot = null;

        if (previousSlot == null)
            return;
        
        RemoveFromGrid(previousSlot.row, previousSlot.col);
        
        CurrentShipStats.Instance.RemoveSubsystem(subsystem);
    }

    public void Place()
    {
        SetAllItemsBlockRaycasts(true);
        isDragging = false;
        currentDraggedItem = null;
        
        GridManager.Instance.ClearPlacementPreview();

        if (hoveredSlot == null)
        {
            Destroy(gameObject);
            return;
        }
        
        if (!CanPlace(hoveredSlot.row, hoveredSlot.col))
        {
            if (previousSlot == null)
            {
                Destroy(gameObject);
                return;
            }
            
            Debug.LogWarning("HoveredSlot was null, cannot place item");
            ReturnToPreviousPosition();
            return;
        }
        
        PlaceOnGrid(hoveredSlot.row, hoveredSlot.col);
        CurrentShipStats.Instance.AddSubsystem(subsystem);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            RemoveFromGrid(currentSlot.row, currentSlot.col);
            Destroy(gameObject);
            return;
        }
        
        if (isDragging)
        {
            Place();
        }
        else
        {
            PickUp();
        }
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
}
