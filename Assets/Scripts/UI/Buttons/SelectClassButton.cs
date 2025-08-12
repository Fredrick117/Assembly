using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectClassButton : MonoBehaviour
{
    public void OnSelectTypeClicked(GameObject selectionMenu)
    {
        selectionMenu.SetActive(true);
    }
}
