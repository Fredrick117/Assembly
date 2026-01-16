using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

[Serializable]
public class ItemWrapper
{
    public ItemData data;
    public bool isRotated;
    
    public ItemWrapper(ItemData baseData)
    {
        data = baseData;
        isRotated = false;
    }
    
    public bool[,] GetShape()
    {
        bool[,] original = data.GetBaseShape();

        if (!isRotated)
        {
            return original;
        }
        
        bool[,] rotated = new bool[3, 3]; 

        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                rotated[y, 2 - x] = original[x, y];
            }
        }

        return rotated;
    }
}

public class Item : MonoBehaviour, IPointerClickHandler
{
    public static Item currentDraggedItem;
    
    private bool isDragging = false;
    private CanvasGroup canvasGroup;

    //public ItemData data;
    public ItemWrapper wrapper;
    public Subsystem subsystem;

    [HideInInspector]
    // The slot on the grid that previously held this item
    public GridSlot previousSlot;
    // The slot on the grid that this item is currently occupying
    public GridSlot currentSlot;
    // The slot on the grid that this item is currently hovering over (only valid when item is being dragged)
    private GridSlot hoveredSlot;

    private Image icon;

    private bool rotationBeforePickup;

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

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Rotate();
            }
            
            if (Input.GetMouseButtonDown(0))
            {
                Place();
            }
        }
    }

    private void Rotate()
    {
        wrapper.isRotated = !wrapper.isRotated;
        
        if (hoveredSlot != null)
            GridManager.Instance.ShowPlacementPreview(hoveredSlot.row, hoveredSlot.col, wrapper);

        RectTransform rect = icon.GetComponent<RectTransform>();
        rect.rotation = wrapper.isRotated ? Quaternion.Euler(0f, 0f, 90f) : Quaternion.identity;
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

    private bool CanPlace(GridSlot homeSlot)
    {
        if (hoveredSlot == null || hoveredSlot.parentGrid == null || homeSlot == null)
            return false;

        GridManager targetGrid = hoveredSlot.parentGrid;

        for (int r = 0; r < 3; r++)
        {
            for (int c = 0; c < 3; c++)
            {
                if (!wrapper.GetShape()[r, c])
                    continue;

                int rowOffset = (r + homeSlot.row) - 1;
                int colOffset = (c + homeSlot.col) - 1;

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
        icon.transform.rotation = wrapper.isRotated ? Quaternion.Euler(0f, 0f, 90f) : Quaternion.identity;

        for (int r = 0; r < 3; r++)
        {
            for (int c = 0; c < 3; c++)
            {
                if (!wrapper.GetShape()[r, c])
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
        GridManager targetGrid = GridManager.Instance;

        for (int r = 0; r < 3; r++)
        {
            for (int c = 0; c < 3; c++)
            {
                if (!wrapper.GetShape()[r, c])
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
        wrapper.isRotated = rotationBeforePickup;
        
        if (previousSlot == null)
        {
            Debug.LogError("No previous slot! Destroying item");
            Destroy(gameObject);
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

        rotationBeforePickup = wrapper.isRotated;
        RemoveFromGrid(previousSlot.row, previousSlot.col);
        
        CurrentShipStats.Instance.RemoveSubsystem(subsystem);
    }

    public void Place()
    {
        SetAllItemsBlockRaycasts(true);
        isDragging = false;
        currentDraggedItem = null;
        
        GridManager.Instance.ClearPlacementPreview();

        bool canPlace = CanPlace(hoveredSlot);
        if (previousSlot == null && !canPlace)
        {
            Destroy(gameObject);
            return;
        }
        
        if (!canPlace)
        {
            Debug.LogWarning("Cannot place!");
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
            CurrentShipStats.Instance.RemoveSubsystem(subsystem);
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
