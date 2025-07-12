using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ArmorMaterial
{
    Cardboard,
    Steel,
    Diamond,
}

[CreateAssetMenu(fileName = "NewArmor", menuName = "Subsystem/Armor")]
public class Armor : Subsystem
{
    public int rating;
    public float massIncrease;
    public ArmorMaterial armorMaterial;
}
