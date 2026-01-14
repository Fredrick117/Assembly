using UnityEngine;
using System;

[CreateAssetMenu(fileName = "NewItem", menuName = "Items/Item")]
public class ItemData : ScriptableObject
{
    [Header("Shape (3x3 Grid)")]
    [SerializeField]
    private bool[] shape = new bool[9];

    public bool[,] GetBaseShape()
    {
        bool[,] result = new bool[3, 3];
        for (int i = 0; i < 9; i++)
        {
            result[i / 3, i % 3] = shape[i];
        }
        return result;
    }
    
    // public bool IsCellFilled(int row, int col)
    // {
    //     return shape[row * 3 + col];
    // }
}