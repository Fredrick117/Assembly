using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DamageType
{ 
    Energy,
    Kinetic
}

public enum WeaponType
{ 
    Missile,
    PointDefense,
    StrikeCraft,
    Torpedo,
    Projectile,
    Railgun,
    Laser,
    Plasmathrower,
}

[CreateAssetMenu(fileName = "NewWeapon", menuName = "Subsystem/Weapon")]
public class Weapon : Subsystem
{
    public DamageType damageType;
    public WeaponType weaponType;
    public int damagePerSecond;
}
