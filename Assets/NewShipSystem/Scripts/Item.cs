using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Item : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [HideInInspector]
    public static Item currentDraggedItem;

    private bool isDragging = false;
    private RectTransform rectTransform;
    private Canvas canvas;
    private CanvasGroup canvasGroup;

    public ItemData data;

    [HideInInspector]
    public GridSlot currentSlot;
    private GridSlot hoveredSlot;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isDragging)
        {
            transform.position = eventData.position;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = false;
        isDragging = true;
        Item.currentDraggedItem = this;

        hoveredSlot.isOccupied = false;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        isDragging = false;
        Item.currentDraggedItem = null;

        if (hoveredSlot != null && !hoveredSlot.isOccupied)
        {
            transform.position = hoveredSlot.transform.position;
            hoveredSlot.image.color = GridSlot.defaultColor;
            hoveredSlot.isOccupied = true;
        }
    }

    public void SetHoveredSlot(GridSlot slot)
    {
        hoveredSlot = slot;
    }
}
