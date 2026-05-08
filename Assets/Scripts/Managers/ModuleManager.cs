using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModuleManager : MonoBehaviour
{
    public static ModuleManager Instance { get; private set; }

    // A preview of the object that is going to be placed
    [HideInInspector]
    public GameObject ghostModule;

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

    public void SetGhostModule(GameObject prefab)
    {
        ClearGhostModule();

        ghostModule = GameObject.Instantiate(prefab);
        ghostModule.GetComponent<DraggableModule>().Ghostify();
    }

    public void ClearGhostModule()
    {
        if (ghostModule != null)
        {
            Destroy(ghostModule);
            ghostModule = null;
        }
    }
}
