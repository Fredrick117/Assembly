using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnPanel : MonoBehaviour
{
    [SerializeField]
    private GameObject buttonPrefab;

    void Start()
    {
        List<GameObject> allPlaceableModules = Resources.LoadAll<GameObject>("ShipModulePrefabs").ToList();

        foreach (GameObject module in allPlaceableModules)
        {
            GameObject button = GameObject.Instantiate(buttonPrefab, transform);
            button.GetComponent<SpawnButton>().prefabToSpawn = module;
        }
    }
}
