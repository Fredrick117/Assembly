using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SpawnMenuSectionButton : MonoBehaviour, IPointerClickHandler
{
    public GameObject prefabToSpawn;

    public void OnPointerClick(PointerEventData eventData)
    {
        GameManager.Instance.SpawnShipSection(prefabToSpawn);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
