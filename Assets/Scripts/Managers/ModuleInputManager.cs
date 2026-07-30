using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModuleInputManager : MonoBehaviour
{
    private void Update()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Collider2D hitCollider = Physics2D.OverlapPoint(mousePosition);

        ModuleStateController moduleState = hitCollider.GetComponent<ModuleStateController>();

        if (moduleState == null)
        {
            return;
        }

        if (hitCollider != null)
        {
            moduleState.SetIsMouseOver(true);
        }
        else if (hitCollider == null && moduleState.GetIsMouseOver() == true)
        {
            moduleState.SetIsMouseOver(false);
        }

        // TODO: ADD INPUT HANDLING FOR MOUSE CLICKS
    }
}

public class ModuleInput
{
    public Vector2 mousePosition;
    
    public bool isRightMouseDown;
    public bool isRightMouseUp;

    public bool isLeftMouseDown;
    public bool isLeftMouseUp;

    public bool isSpacebarPressed;

    public bool isMouseOver;
}
