using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ShipSectionType
{
    Front,
    Middle1,
    Rear,
}

public enum ShipClass
{
    Corvette,
    Frigate,
    Destroyer,
    Carrier,
}

public class SpawnButton : MonoBehaviour
{
    public GameObject prefabToSpawn;

    public void OnSpawnButtonPressed()
    {
        GameManager.Instance.SpawnShipSection(prefabToSpawn);
    }
}
