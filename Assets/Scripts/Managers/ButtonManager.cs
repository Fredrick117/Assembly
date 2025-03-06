using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    public void OnCloseClicked(GameObject uiElement)
    {
        uiElement.SetActive(false);
    }

    public void OnSelectTypeClicked(GameObject selectionMenu)
    {
        selectionMenu.SetActive(true);
    }
}
