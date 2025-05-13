using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField]
    private DraggableManager draggableManager;

    [SerializeField]
    private ShipRequestManager shipRequestManager;

    [SerializeField]
    private ShipManager shipManager;

    [HideInInspector]
    public int credits = 5000;
    public TMP_Text creditsText;

    public GameObject subsystemSelectionMenu;

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

    public void SpawnObjectFromButton(GameObject newObject)
    {
        GameObject spawnedObject = GameObject.Instantiate(newObject, (Vector2)Input.mousePosition, Quaternion.identity);

        if (spawnedObject == null)
        {
            Debug.LogError("SpawnedObject was null!");
            return;
        }
        
        // TODO: what if object doesn't have Draggable component?
        draggableManager.currentlyDraggedObject = spawnedObject.GetComponent<Draggable>();
        DraggableManager.Instance.currentlyDraggedObject = spawnedObject.GetComponent<Draggable>();
        spawnedObject.GetComponent<Draggable>().isDragging = true;
    }

    public void UpdateCredits(int amount)
    {
        credits += amount;

        creditsText.text = credits.ToString();
    }
}
