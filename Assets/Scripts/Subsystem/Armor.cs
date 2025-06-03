using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ArmorMaterial
{
    
}

[CreateAssetMenu(fileName = "NewArmor", menuName = "Subsystem")]
public class Armor : Subsystem
{
    public int armor;
    public ArmorMaterial armorMaterial;
}
