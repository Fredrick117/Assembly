using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ship Data", menuName = "Ship/ShipData")]
public class ShipData : ScriptableObject
{
    [Header("Layout")]
    [SerializeField] private int[] layout = new int[36];

    public int[,] GetLayout()
    {
        int[,] result = new int[3, 12];
        for (int i = 0; i < 36; i++)
        {
            result[i / 3, i % 12] = layout[i];
        }

        return result;
    }

    public SlotType GetSlotType(int row, int col)
    {
        return (SlotType)layout[row * 3 + col];
    }
}
