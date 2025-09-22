using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class SpawnButton : MonoBehaviour, IPointerDownHandler
{
    public GameObject objectToSpawn;

    public ModuleBase module;

    private TMP_Text buttonText;

    private void Start()
    {
        buttonText = GetComponentInChildren<TMP_Text>();

        if (objectToSpawn != null)
        {
            buttonText.text = module.name;
        }
        else
        {
            buttonText.text = "null";
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //GameManager.Instance.SpawnObjectFromButton(objectToSpawn);
        GameManager.Instance.SelectModule(module);
    }
}
