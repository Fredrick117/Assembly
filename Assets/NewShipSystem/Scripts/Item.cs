using System;
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

    [SerializeField]
    private ItemData data;
    private ItemData runtimeData;

    [HideInInspector]
    // The slot on the grid that previously held this item
    public GridSlot previousSlot;
    // The slot on the grid that this item is currently occupying
    public GridSlot currentSlot;
    // The slot on the grid that this item is currently hovering over (only valid when item is being dragged)
    private GridSlot hoveredSlot;

    private Image icon;

    // The item's current rotation; can be 0, 90, 180, or 270
    private int currentRotation = 0;

    public static event Action<Item> OnPickUpItem;
    public static event Action<Item> OnDropItem;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
        icon = GetComponentInChildren<Image>();
        
        // Data is null in Start()...?
        if (data == null || icon == null)
        {
            Debug.LogError("Start: data or icon were invalid!");
            return;
        }

        runtimeData = Instantiate(data);
        icon.sprite = data.itemSprite;
    }

    void Start()
    {
        icon.alphaHitTestMinimumThreshold = 0.1f;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isDragging)
        {
            Rotate();
        }
    }

    void OnValidate()
    {
        GetComponentInChildren<Image>().sprite = data.itemSprite;
    }

    public ItemData GetRuntimeData()
    {
        return this.runtimeData;
    }

    public void SetRuntimeData(ItemData inData)
    {
        this.runtimeData = inData;
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

    public void Rotate()
    {
        RectTransform rect = GetComponentInChildren<RectTransform>();
        rect.Rotate(new Vector3(0, 0, 90f));
        currentRotation = (currentRotation + 90) % 360;

        bool[,] rotatedMatrix = new bool[3, 3];

        var source = runtimeData.GetShape();
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                rotatedMatrix[i, j] = source[j, (2 - i)];
            }
        }

        runtimeData.SetShape(rotatedMatrix);

        if (hoveredSlot != null)
        {
            hoveredSlot.parentGrid.ShowPlacementPreview(hoveredSlot.row, hoveredSlot.col, runtimeData);
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

    public bool CanPlace(int row, int col, GridManager grid)
    {
        for (int r = 0; r < 3; r++)
        {
            for (int c = 0; c < 3; c++)
            {
                if (!this.runtimeData.IsCellFilled(r, c))
                    continue;

                int rowOffset = (r + row) - 1;
                int colOffset = (c + col) - 1;

                if (!grid.IsInBounds(rowOffset, colOffset))
                    return false;

                GridSlot slot = grid.GridSlots[rowOffset, colOffset];

                if (slot.isOccupied)
                    return false;
            }
        }

        return true;
    }

    public void PlaceOnGrid(int row, int col, GridManager targetGrid)
    {
        transform.position = targetGrid.GetSlot(row, col).GetComponent<RectTransform>().position;
        print($"Placed item at X: {transform.position.x}, Y: {transform.position.y}");

        for (int r = 0; r < 3; r++)
        {
            for (int c = 0; c < 3; c++)
            {
                if (!runtimeData.IsCellFilled(r, c))
                    continue;

                int rowOffset = (r + row) - 1;
                int colOffset = (c + col) - 1;

                if (!targetGrid.IsInBounds(rowOffset, colOffset))
                    continue;

                GridSlot neighbor = targetGrid.GridSlots[rowOffset, colOffset];

                neighbor.isOccupied = true;
                neighbor.currentItem = this;
                neighbor.image.color = GridSlot.defaultColor;
            }
        }

        if (targetGrid.type == GridType.Ship)
        {
            print("Placed on ship!");
            data.subsystemData.ApplyToShip(ShipStats.Instance);
        }

        currentSlot = targetGrid.GridSlots[row, col];
        previousSlot = currentSlot;
    }

    private void PickUpFromGrid(int row, int col)
    {
        if (previousSlot == null || previousSlot.parentGrid == null)
        {
            Debug.LogError("PickUpFromGrid: previousSlot or parentGrid was null!");
            return;
        }

        GridManager targetGrid = previousSlot.parentGrid;
        
        transform.SetParent(canvas.transform);

        for (int r = 0; r < 3; r++)
        {
            for (int c = 0; c < 3; c++)
            {
                if (!runtimeData.IsCellFilled(r, c))
                    continue;

                int rowOffset = (r + row) - 1;
                int colOffset = (c + col) - 1;

                if (!targetGrid.IsInBounds(rowOffset, colOffset))
                    continue;

                GridSlot neighbor = targetGrid.GridSlots[rowOffset, colOffset];

                neighbor.isOccupied = false;
                neighbor.currentItem = null;
                neighbor.image.color = GridSlot.defaultColor;
            }
        }
        
        if (targetGrid.type == GridType.Ship)
        {
            print("Picked up from ship!");
            data.subsystemData.RemoveFromShip(ShipStats.Instance);
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
        OnDropItem?.Invoke(null);

        SetAllItemsBlockRaycasts(true);
        isDragging = false;
        Item.currentDraggedItem = null;
    
        if (hoveredSlot == null)
        {
            Debug.LogWarning("hoveredSlot was null!");
            ReturnToPreviousPosition();
            return;
        }
        
        GridManager targetGrid = hoveredSlot.parentGrid;
        if (targetGrid != null)
        {
            targetGrid.ClearPlacementPreview();
        }

        if (!CanPlace(hoveredSlot.row, hoveredSlot.col, hoveredSlot.parentGrid))
        {
            Debug.LogWarning("Can't place!");
            ReturnToPreviousPosition();
            return;
        }

        PlaceOnGrid(hoveredSlot.row, hoveredSlot.col, hoveredSlot.parentGrid);
    }
}
