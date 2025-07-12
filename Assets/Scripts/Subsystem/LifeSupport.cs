using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum AtmosphereType
{
    Nitrogen_Oxygen,
    Methane,
    Carbon_Dioxide,
}

[CreateAssetMenu(fileName = "NewLifeSupport", menuName = "Subsystem/LifeSupport")]
public class LifeSupport : Subsystem
{
    public AtmosphereType atmosphereType;
}
