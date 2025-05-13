using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SpawnPanel : MonoBehaviour
{
    private string prefabsPath = "Assets/Prefabs/ShipModules";

    public GameObject buttonPrefab;

    // Start is called before the first frame update
    void Awake()
    {
        string[] guids = AssetDatabase.FindAssets("t:prefab", new string[] {prefabsPath});

        foreach (string guid in guids)
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);

            GameObject spawnButton = GameObject.Instantiate(buttonPrefab, transform);
            spawnButton.GetComponent<SpawnButton>().SetObjectToSpawn(prefab);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
