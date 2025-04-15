using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewShielding", menuName = "Subsystem/Shielding")]
public class Shielding : Subsystem
{
    public int shieldStrength;
    public int rechargeSpeed;
}
