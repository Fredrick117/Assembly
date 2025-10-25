using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FTLTier
{
    Short,
    Medium,
    Long
}

[CreateAssetMenu(fileName = "New FTL Drive", menuName = "Subsystem/FTLDrive")]
public class FTLDrive : Subsystem
{
    public FTLTier tier;
}
