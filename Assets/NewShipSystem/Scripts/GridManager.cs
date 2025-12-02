using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance { get; private set; }

    [SerializeField]
    private GameObject slotPrefab;

    [Header("Grid Settings")]
    public int rows;
    public int columns;

    [Header("Item Mask Settings")]
    public int maskWidth;
    public int maskHeight;

    private GridLayoutGroup gridLayoutGroup;

    public GridSlot[,] gridSlots { get; private set; }

    private List<GridSlot> previewedSlots = new List<GridSlot>();

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        gridLayoutGroup = GetComponent<GridLayoutGroup>();
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

        if (data == null)
            return;

        bool outOfBounds = false;

        for (int row = 0; row < 3; row++)
        {
            for (int col = 0; col < 3; col++)
            {
                if (!data.IsCellFilled(row, col))
                    continue;

                int r = startRow + row;
                int c = startCol + col;

                if (!IsInBounds(r, c))
                {
                    outOfBounds = true;
                    continue;
                }

                GridSlot slot = gridSlots[row, col];
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
        gridSlots = new GridSlot[rows, columns];

        gridLayoutGroup.constraintCount = rows;

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                GameObject slot = GameObject.Instantiate(slotPrefab, transform);
                slot.GetComponent<GridSlot>().row = i;
                slot.GetComponent<GridSlot>().col = j;
                gridSlots[i, j] = slot.GetComponent<GridSlot>();
            }
        }
    }

    private void ClearGrid()
    {
        for (int i = 0; i < columns * rows; i++)
        {
            GameObject.Destroy(transform.GetChild(i).gameObject);
        }
    }
}