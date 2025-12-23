using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GridType
{
    Inventory,
    Ship
}

public class GridManager : MonoBehaviour
{
    [SerializeField] private GameObject slotPrefab;
    
    [Header("Debugging")]
    [SerializeField] private GameObject debugCirclePrefab;
    [SerializeField] private Canvas canvas;

    [Header("Grid Settings")]
    public int rows;
    public int columns;
    public GridType type;

    [Header("Item Mask Settings")]
    public int maskWidth;
    public int maskHeight;

    private GridLayoutGroup gridLayoutGroup;

    public GridSlot[,] GridSlots { get; set; }

    private List<GridSlot> previewedSlots = new List<GridSlot>();

    //public static event Action<GridManager> OnGridGenerated;

    void Awake()
    {
        gridLayoutGroup = GetComponent<GridLayoutGroup>();
    }

    void Start()
    {
        gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedRowCount;
        GenerateGrid();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ClearGrid();
            rows = 5;
            columns = 18;
            GenerateGrid();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ClearGrid();
            rows = 7;
            columns = 22;
            GenerateGrid();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ClearGrid();
            rows = 10;
            columns = 26;
            GenerateGrid();
        }
    }

    public bool IsInBounds(int row, int column)
    {
        return (row >= 0 && row < rows && column >= 0 && column < columns);
    }

    public void ShowPlacementPreview(int startRow, int startCol, ItemData data)
    {
        ClearPlacementPreview();

        if (!data)
            return;

        bool outOfBounds = false;

        for (int rowOffset = 0; rowOffset < 3; rowOffset++)
        {
            for (int colOffset = 0; colOffset < 3; colOffset++)
            {
                if (!data.IsCellFilled(rowOffset, colOffset))
                    continue;

                int r = (startRow + rowOffset) - 1;
                int c = (startCol + colOffset) - 1;

                if (!IsInBounds(r, c))
                {
                    outOfBounds = true;
                    continue;
                }

                GridSlot slot = GridSlots[r, c];
                slot.Preview(!slot.isOccupied);
                previewedSlots.Add(slot);
            }
        }

        if (outOfBounds && previewedSlots.Count > 0)
        {
            foreach (var slot in previewedSlots)
            {
                slot.Preview(false);
            }
        }
    }

    public GridSlot GetSlot(int row, int col)
    {
        if (!IsInBounds(row, col))
        {
            Debug.LogError("GetSlot: invalid row/col!");
            return null;
        }

        return GridSlots[row, col];
    }

    public void ClearPlacementPreview()
    {
        if (previewedSlots == null || previewedSlots.Count == 0)
            return;

        foreach (var slot in previewedSlots)
        {
            if (slot != null)
                slot.ResetColor();
        }

        previewedSlots.Clear();
    }

    private void GenerateGrid()
    {
        GridSlots = new GridSlot[rows, columns];
        gridLayoutGroup.constraintCount = rows;

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                GameObject slot = GameObject.Instantiate(slotPrefab, transform);
                GridSlot gridSlot = slot.GetComponent<GridSlot>();
                gridSlot.row = i;
                gridSlot.col = j;
                gridSlot.parentGrid = this;
                GridSlots[i, j] = gridSlot;
            }
        }

        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());

        //OnGridGenerated?.Invoke(this);
    }

    private void ClearGrid()
    {
        for (int i = 0; i < columns * rows; i++)
        {
            GameObject.Destroy(transform.GetChild(i).gameObject);
        }
    }
}