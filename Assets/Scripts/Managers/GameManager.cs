using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [HideInInspector]
    public int credits = 5000;

    public TMP_Text creditsText;

    public GameObject subsystemSelectionMenu;

    public Transform gameOverPanel;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        creditsText.text = credits.ToString();

        DontDestroyOnLoad(gameObject);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void ShowSubsystemMenu()
    {
        subsystemSelectionMenu.SetActive(true);
    }

    public void HideSubsystemMenu()
    {
        subsystemSelectionMenu.SetActive(false);
    }

    public void ShowGameOverScreen()
    {
        gameOverPanel.gameObject.SetActive(true);
    }
}
