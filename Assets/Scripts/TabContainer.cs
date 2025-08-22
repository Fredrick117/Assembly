using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TabContainer : MonoBehaviour
{
    public List<Transform> tabs = new List<Transform>();

    public SpawnPanelTab GetTabByName(string name)
    {
        Transform t = tabs.Where(tab => tab.gameObject.GetComponent<SpawnPanelTab>().tabName == name).SingleOrDefault();
        return t.gameObject.GetComponent<SpawnPanelTab>();
    }
}
