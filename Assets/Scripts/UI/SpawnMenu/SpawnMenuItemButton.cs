using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SpawnMenuItemButton : MonoBehaviour, IPointerClickHandler
{
    public GameObject objectToSpawn;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (GameManager.Instance.isDragging)
        {
            GameObject.Destroy(GameManager.Instance.currentlyDraggedObject);
        }

        GameManager.Instance.currentlyDraggedObject = GameObject.Instantiate(objectToSpawn, Camera.main.ScreenToWorldPoint(Input.mousePosition), Quaternion.identity);
        GameManager.Instance.isDragging = true;
    }
}
