using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridManager : MonoBehaviour
{
    [SerializeField]
    private GameObject slotPrefab;

    private int gridRows = 0;
    private int gridColumns = 0;

    private GridLayoutGroup gridLayoutGroup;

    public GridSlot[,] gridSlots { get; private set; }

    private List<GridSlot> previewedSlots = new List<GridSlot>();
    
    public static GridManager Instance { get; private set; }

    void Awake()
    {
        if (Instance != this && Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
        
        gridLayoutGroup = GetComponent<GridLayoutGroup>();
    }

    void Start()
    {
        gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedRowCount;
    }

    public bool IsInBounds(int row, int column)
    {
        return (row >= 0 && row < gridRows && column >= 0 && column < gridColumns);
    }

    public void ShowPlacementPreview(int startRow, int startCol, ItemWrapper data)
    {
        ClearPlacementPreview();

        if (data == null)
            return;

        bool outOfBounds = false;

        for (int rowOffset = 0; rowOffset < 3; rowOffset++)
        {
            for (int colOffset = 0; colOffset < 3; colOffset++)
            {
                if (!data.GetShape()[rowOffset, colOffset])
                    continue;

                int r = (startRow + rowOffset) - 1;
                int c = (startCol + colOffset) - 1;

                if (!IsInBounds(r, c))
                {
                    outOfBounds = true;
                    continue;
                }

                GridSlot slot = gridSlots[r, c];
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

        return gridSlots[row, col];
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

    public void GenerateGrid(int rows, int columns)
    {
        ClearGrid();
        
        gridRows = rows;
        gridColumns = columns;
        
        gridSlots = new GridSlot[rows, columns];
        gridLayoutGroup.constraintCount = rows;

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                GameObject slot = Instantiate(slotPrefab, transform);
                GridSlot gridSlot = slot.GetComponent<GridSlot>();
                gridSlot.row = i;
                gridSlot.col = j;
                gridSlot.parentGrid = this;
                gridSlots[i, j] = slot.GetComponent<GridSlot>();
            }
        }
    }

    public void ClearGrid()
    {
        if (transform.childCount == 0)
            return;
        
        for (int i = 0; i < gridColumns * gridRows; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }
}