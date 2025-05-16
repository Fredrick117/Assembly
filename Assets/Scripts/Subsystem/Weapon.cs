using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{ 
    Missile,
    PointDefense,
    StrikeCraft,
    Torpedo,
    Projectile,
    Railgun,
    LaserCannon,
    LaserBeam,
    Plasmathrower,
    Nuke,
}

[CreateAssetMenu(fileName = "NewWeapon", menuName = "Subsystem/Weapon")]
public class Weapon : Subsystem
{
    public WeaponType weaponType;
    //public int damagePerSecond;
}
