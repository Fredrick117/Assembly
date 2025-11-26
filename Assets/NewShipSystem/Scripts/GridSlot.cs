using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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

    private TMP_Text debugText;

    [Header("Visualization")]
    public static Color validColor = new Color(0, 1, 0, 0.3f);
    public static Color invalidColor = new Color(1, 0, 0, 0.3f);
    public static Color defaultColor = new Color(0, 0, 0, 0.3f);

    void Awake()
    {
        debugText = GetComponentInChildren<TMP_Text>();
        image = GetComponent<Image>();
    }

    void Start()
    {
        defaultColor = image.color;
        debugText.text = $"({row}, {col})";
        debugText.gameObject.SetActive(showPositions);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (Item.currentDraggedItem != null)
        {
            Item.currentDraggedItem.SetHoveredSlot(this);

            if (isOccupied)
                image.color = invalidColor;
            else
                image.color = validColor;
        }

        GridManager.Instance.HighlightSlots();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (Item.currentDraggedItem != null)
        {
            Item.currentDraggedItem.SetHoveredSlot(null);
            image.color = defaultColor;
        }
    }

    public void Highlight()
    {
        image.color = isOccupied ? invalidColor : validColor;
    }

    public void ResetColor()
    {
        image.color = defaultColor;
    }
}
