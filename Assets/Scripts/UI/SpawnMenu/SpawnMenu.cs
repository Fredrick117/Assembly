using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnMenu : MonoBehaviour
{
    public GameObject sectionsContent;
    public GameObject weaponsContent;
    public GameObject subsystemsContent;

    public GameObject sectionsTab;
    public GameObject weaponsTab;
    public GameObject subsystemsTab;

    public void EnableContent(string content)
    {
        if (content == "Sections")
        {
            sectionsContent.SetActive(true);
            weaponsContent.SetActive(false);
            subsystemsContent.SetActive(false);

            sectionsTab.GetComponent<Image>().color = Color.black;
        }
        else if (content == "Weapons")
        {
            sectionsContent.SetActive(false);
            weaponsContent.SetActive(true);
            subsystemsContent.SetActive(false);
        }
        else if (content == "Subsystems")
        {
            sectionsContent.SetActive(false);
            weaponsContent.SetActive(false);
            subsystemsContent.SetActive(true);
        }
    }
}
