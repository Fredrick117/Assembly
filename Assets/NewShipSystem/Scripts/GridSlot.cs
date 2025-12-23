using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum SlotType
{
    Any,
    Thruster,
    Weapon,
    Invalid,
}

public class GridSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [HideInInspector]
    public Image image;
    [HideInInspector]
    public bool isOccupied = false;
    [HideInInspector]
    public Item currentItem = null;

    [SerializeField]
    private bool showPositions = false;

    [HideInInspector]
    public int row;
    [HideInInspector]
    public int col;

    [HideInInspector]
    public GridManager parentGrid;

    private TMP_Text debugText;

    [Header("Visualization")]
    public static Color validColor = new Color(0, 1, 0, 0.3f);
    public static Color invalidColor = new Color(1, 0, 0, 0.3f);
    public static Color defaultColor = new Color(0, 0, 0, 0.3f);
    public static Color occupiedColor = new Color(1, 1, 0, 0.3f);

    [HideInInspector] public SlotType type = SlotType.Any;
    
    public static event Action<GridSlot> OnSlotEnter;
    public static event Action<GridSlot> OnSlotExit;

    void Awake()
    {
        debugText = GetComponentInChildren<TMP_Text>();
        image = GetComponent<Image>();
    }

    void Start()
    {
        defaultColor = image.color;

        if (debugText != null)
        {
            debugText.text = $"({row}, {col})";
            debugText.gameObject.SetActive(showPositions);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (Item.currentDraggedItem == null)
        {
            return;
        }
        
        OnSlotEnter?.Invoke(this);
        Item.currentDraggedItem.SetHoveredSlot(this);
        parentGrid.ShowPlacementPreview(row, col, Item.currentDraggedItem.GetRuntimeData());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (Item.currentDraggedItem == null)
        {
            return;
        }
        
        OnSlotExit?.Invoke(null);

        Item.currentDraggedItem.SetHoveredSlot(null);
        image.color = defaultColor;

        parentGrid.ClearPlacementPreview();
    }

    public void ResetColor()
    {
        image.color = defaultColor;
    }

    public void Preview(bool isValid)
    {
        image.color = isValid ? validColor : invalidColor;
    }
}
