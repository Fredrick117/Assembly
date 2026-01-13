using UnityEngine;
using System;

[CreateAssetMenu(fileName = "NewItem", menuName = "Items/Item")]
public class ItemData : ScriptableObject
{
    //[Header("Item Properties")]
    //public string itemName;
    //public Sprite itemSprite;

    [Header("Shape (3x3 Grid)")]
    [SerializeField]
    private bool[] shape = new bool[9];

    public bool IsCellFilled(int row, int col)
    {
        return shape[row * 3 + col];
    }
}