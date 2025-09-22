using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ModuleBase", menuName = "ScriptableObjects/ModuleBase")]
public class ModuleBase : ScriptableObject
{
    public Vector2Int size;
    public Sprite sprite;
}
