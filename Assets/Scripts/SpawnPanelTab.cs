using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class SpawnPanelTab : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private SpawnPanel spawnPanel;

    public string tabName = "NULL";

    [SerializeField]
    private TMP_Text tabText;

    public bool isSelected = false;

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        tabText.text = tabName;

        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        spawnPanel.SetSelectedTab(tabName);
    }

    public void SetSelectedState(bool newState)
    {
        isSelected = newState;

        if (newState == true)
            spriteRenderer.color = Color.white;
        else
            spriteRenderer.color = Color.grey;
    }
}
