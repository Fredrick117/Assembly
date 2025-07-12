using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSubsystem", menuName = "Subsystem/BaseSubsystem")]
public class Subsystem : ScriptableObject
{
    public string displayName;
    public string description;
    public float powerDraw;
    public int mass;
    //public int price;
    public Sprite icon;
}