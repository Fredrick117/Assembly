using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModuleManager : MonoBehaviour
{
    public static ModuleManager Instance { get; private set; }

    public GameObject shipCore;

    // A preview of the object that is going to be placed
    [HideInInspector]
    public GameObject ghostModule;

    [HideInInspector]
    public GameObject hoveredModule;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Update()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Collider2D hitCollider = Physics2D.OverlapPoint(mousePosition);

        if (hitCollider == null)
        {
            print("no longer hovering!");
            if (hoveredModule != null)
            {
                ModuleStateController previouslyHoveredModuleState = hoveredModule.GetComponent<ModuleStateController>();
                previouslyHoveredModuleState.SetIsMouseOver(false);
                hoveredModule = null;
            }

            return;
        }

        ModuleStateController moduleState = hitCollider.GetComponent<ModuleStateController>();

        if (moduleState == null)
        {
            return;
        }

        if (hitCollider != null)
        {
            print("hovering!");
            hoveredModule = hitCollider.gameObject;
            moduleState.SetIsMouseOver(true);
        }
    }

    public void SetGhostModule(GameObject module)
    {
        ClearGhostModule();

        ghostModule = module;
        ghostModule.GetComponent<DraggableModule>().Ghostify();
        ghostModule.GetComponent<DraggableModule>().isDragging = true;
    }

    public void ClearGhostModule()
    {
        if (ghostModule != null)
        {
            Destroy(ghostModule);
            ghostModule = null;
        }
    }

    public GameObject SpawnModuleAtMousePosition(GameObject modulePrefab)
    {
        return Instantiate(modulePrefab, Camera.main.ScreenToWorldPoint(Input.mousePosition), Quaternion.identity);
    }
}
