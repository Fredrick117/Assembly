using UnityEngine;
using System;
using System.Linq;

[CreateAssetMenu(fileName = "NewItem", menuName = "Items/Item")]
public class ItemData : ScriptableObject
{
    [Header("Item Properties")]
    public string itemName;
    public Sprite itemSprite;
    public Subsystem subsystemData;

    [Header("Shape")]
    [SerializeField]
    private bool[] shape = new bool[9];

    public bool[,] GetShape()
    {
        bool[,] result = new bool[3, 3];
        for (int i = 0; i < 9; i++)
        {
            result[i / 3, i % 3] = shape[i];
        }
        return result;
    }

    public void SetShape(bool[,] inShape)
    {
        shape = inShape.Cast<bool>().ToArray();
    }

    public bool IsCellFilled(int row, int col)
    {
        return shape[row * 3 + col];
    }

    public bool GetCell(int index) => shape[index];
    public void SetCell(int index, bool value) => shape[index] = value;
}