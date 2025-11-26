using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridManager : MonoBehaviour
{
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

    private void GenerateGrid()
    {
        gridSlots = new GridSlot[rows, columns];

        gridLayoutGroup.constraintCount = rows;

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                GameObject slot = GameObject.Instantiate(slotPrefab, transform);
                slot.GetComponent<GridSlot>().position = new Vector2Int(i, j);
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