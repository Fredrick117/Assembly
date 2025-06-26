using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class HullSpawnPanel : MonoBehaviour
{
    [SerializeField]
    private GameObject spawnButtonPrefab;

    // Start is called before the first frame update
    void Start()
    {
        GameObject[] hullPrefabs = Resources.LoadAll<GameObject>("HullPrefabs");

        foreach(GameObject hull in hullPrefabs)
        {
            GameObject button = GameObject.Instantiate(spawnButtonPrefab, this.transform);
            SpawnButton buttonData = button.GetComponent<SpawnButton>();

            buttonData.objectToSpawn = hull;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
