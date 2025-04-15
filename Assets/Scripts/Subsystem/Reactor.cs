using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewReactor", menuName = "Subsystem/Reactor")]
public class Reactor : Subsystem
{
    public int powerOutput;
    public string powerType;
}
