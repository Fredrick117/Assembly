using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SpawnPanel : MonoBehaviour
{
    [SerializeField]
    private GameObject spawnButtonPrefab;

    [SerializeField]
    private GameObject content;

    [SerializeField]
    private TabContainer tabs;

    private string selectedTab = "";

    void Start()
    {
        ScriptableObject[] hullPrefabs = Resources.LoadAll<ScriptableObject>("HullPrefabs");

        foreach(ScriptableObject hull in hullPrefabs)
        {
            GameObject button = GameObject.Instantiate(spawnButtonPrefab, content.transform);
            SpawnButton buttonData = button.GetComponent<SpawnButton>();

            buttonData.module = (ModuleBase)hull;
        }
    }

    public void SetSelectedTab(string tabName)
    {
        if (selectedTab == tabName) 
            return;

        tabs.GetTabByName(selectedTab).SetSelectedState(true);
        tabs.GetTabByName(tabName).SetSelectedState(true);

        selectedTab = tabName;
    }
}
