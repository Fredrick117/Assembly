using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToDesignerButton : MonoBehaviour
{
    public void OnClicked()
    {
        SceneManager.LoadScene("ShipDesignScene");
    }
}
