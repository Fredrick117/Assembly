using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    public void OnCloseClicked(GameObject uiElement)
    {
        uiElement.SetActive(false);
    }

    public void OnStartGameClicked()
    {
        SceneManager.LoadScene("ShipDesignScene");
    }

    public void OnQuitClicked()
    {
        Application.Quit();
    }
}
