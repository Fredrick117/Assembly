using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpawnButton : MonoBehaviour
{
    public GameObject prefabToSpawn;

    private TMP_Text buttonText;

    private void Awake()
    {
        buttonText = gameObject.GetComponentInChildren<TMP_Text>();
        buttonText.text = prefabToSpawn.name;
    }

    public void OnButtonClicked()
    {
        Destroy(ModuleManager.Instance.ghostModule);

        GameObject spawnedObject = GameObject.Instantiate(prefabToSpawn);
        ModuleManager.Instance.SetGhostModule(spawnedObject);
        DraggableModule shipModule = spawnedObject.GetComponent<DraggableModule>();

        if (shipModule)
        {
            shipModule.isDragging = true;
        }
    }
}
