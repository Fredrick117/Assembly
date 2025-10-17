using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int startingCredits = 2500000;

    [HideInInspector]
    public int currentCredits;

    public GameObject subsystemSelectionMenu;

    public Transform gameOverPanel;

    public Transform pausePanel;

    [SerializeField]
    private Header headerPanel;

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

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        currentCredits = startingCredits;
        headerPanel.UpdateHeaderText();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pausePanel.gameObject.SetActive(true);
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
