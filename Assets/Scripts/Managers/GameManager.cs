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

    private bool isLastChance = false;

    [SerializeField]
    private GameObject bankruptcyWarningText;

    public GridManager gridManager;

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
    }

    private void Start()
    {
        currentCredits = startingCredits;

        if (headerPanel)
            headerPanel.UpdateHeaderText();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pausePanel.gameObject.SetActive(true);
        }
    }

    private void OnEnable()
    {
        EventManager.onSubmit += CheckForBankruptcy;
    }

    private void OnDisable()
    {
        EventManager.onSubmit -= CheckForBankruptcy;
    }

    public void ShowSubsystemMenu()
    {
        subsystemSelectionMenu.SetActive(true);
    }

    public void HideSubsystemMenu()
    {
        subsystemSelectionMenu.SetActive(false);
    }

    IEnumerator ShowGameOverScreen()
    {
        yield return new WaitForSeconds(1.5f);

        gameOverPanel.gameObject.SetActive(true);
    }

    public void ModifyCredits(int inCredits)
    {
        currentCredits += inCredits;
    }

    private void CheckForBankruptcy()
    {
        if (currentCredits < 0 && !isLastChance)
        {
            bankruptcyWarningText.SetActive(true);
            isLastChance = true;
        }
        else if (currentCredits < 0 && isLastChance)
        {
            StartCoroutine(ShowGameOverScreen());
        }
    }
}
