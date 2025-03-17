using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubsystemSpawnButton : MonoBehaviour
{
    public GameObject subsystemPrefab;

    public void SpawnSubsystem()
    {
        GameObject spawnedSubsystem = GameObject.Instantiate(subsystemPrefab, Input.mousePosition, Quaternion.identity);
    }
}
