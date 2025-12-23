using System;
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

    public GridManager inventoryGrid;

    public GameObject itemPrefab;

    private List<GameObject> testItems = new List<GameObject>();

    [SerializeField]
    private Canvas mainCanvas;

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

        // for (int i = 0; i < 1; i++)
        // {
        //     testItems.Add(CreateItem(Resources.Load<ItemData>("ScriptableObjects/Items/ThrusterItem"), inventoryGrid).gameObject);
        // }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pausePanel.gameObject.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            GameObject item = Instantiate(itemPrefab, mainCanvas.transform);
            if (item)
            {
                Debug_SpawnItemInInventory(item.GetComponent<Item>(), 1, 0, inventoryGrid);
            }
        }
    }

    private void OnEnable()
    {
        EventManager.onSubmit += CheckForBankruptcy;
        //GridManager.OnGridGenerated += OnGridGenerated;
    }

    private void OnDisable()
    {
        EventManager.onSubmit -= CheckForBankruptcy;
        //GridManager.OnGridGenerated -= OnGridGenerated;
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

    private void OnGridGenerated(GridManager grid)
    {
        // if (grid.type == GridType.Inventory)
        // {
        //     for (int i = 0; i < 1; i++)
        //     {
        //         Debug_SpawnItemInInventory(testItems[i].GetComponent<Item>(), 1, i, grid);
        //     }
        // }
    }

    private void Debug_SpawnItemInInventory(Item itemToSpawn, int row, int col, GridManager grid)
    {
        if (itemToSpawn.CanPlace(row, col, grid))
        {
            itemToSpawn.PlaceOnGrid(row, col, grid);
        }
    }

    private Item CreateItem(ItemData itemData, GridManager grid)
    {
        Item item = Instantiate(itemPrefab, mainCanvas.transform).GetComponent<Item>();
        item.SetRuntimeData(itemData);
        return item;
    }
}
