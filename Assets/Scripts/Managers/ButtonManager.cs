using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    [SerializeField]
    private GameObject howToPlayPanel;
    
    public void ToggleHowToPlayPanel(bool isEnabled)
    {
        howToPlayPanel.SetActive(isEnabled);
    }
    
    public void OnCloseClicked(GameObject uiElement)
    {
        uiElement.SetActive(false);
    }

    public void OnStartGameClicked()
    {
        SceneManager.LoadScene("ShipDesignScene");
    }
    
    public void QuitToMain()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitToDesktop()
    {
        Application.Quit();
    }
}
