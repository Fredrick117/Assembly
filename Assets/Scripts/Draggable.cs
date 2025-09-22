using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour
{
    protected bool isDragging = true;

    protected Vector3 offset;

    protected Rigidbody2D rb;

    protected Color originalColor;

    protected readonly Color validPlacementColor = new Color(0, 255, 0, 10);

    protected readonly Color invalidPlacementColor = new Color(255, 0, 0, 10);

    public void SetIsDragging(bool dragging)
    {
        isDragging = dragging;
    }

    public bool GetIsDragging()
    {
        return isDragging;
    }

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
}
