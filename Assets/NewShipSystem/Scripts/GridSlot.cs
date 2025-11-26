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
    public Vector2Int position;

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
        debugText.text = $"({position.x}, {position.y})";
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

            HighlightNeighbors();
        }   
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (Item.currentDraggedItem != null)
        {
            Item.currentDraggedItem.SetHoveredSlot(null);
            image.color = defaultColor;

            DeselectNeighbors();
        }
    }

    public List<GridSlot> GetNeighbors()
    {
        GridManager grid = GameManager.Instance.gridManager;

        List<GridSlot> neighbors = new();

        int maskX = GameManager.Instance.gridManager.maskWidth;

        for (int i = -maskX; i < maskX; i++)
        {
            for (int j = -maskX; j < maskX; j++)
            {
                int x = i + position.x;
                int y = j + position.y;

                if (grid.IsInBounds(x, y))
                {
                    neighbors.Add(grid.gridSlots[x, y]);
                }
            }
        }

        return neighbors;
    }

    public void HighlightNeighbors()
    {
        List<GridSlot> neighbors = GetNeighbors();

        foreach (GridSlot slot in neighbors)
        {
            if (slot.isOccupied)
            {
                slot.image.color = invalidColor;
            }
            else
            {
                slot.image.color = validColor;
            }
        }
    }

    private void DeselectNeighbors()
    {
        List<GridSlot> neighbors = GetNeighbors();

        foreach (GridSlot slot in neighbors)
        {
            slot.image.color = defaultColor;
        }
    }    
}
