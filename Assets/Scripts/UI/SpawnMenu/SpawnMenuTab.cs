using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SpawnMenuTab : MonoBehaviour, IPointerDownHandler
{
    public SpawnMenu spawnMenu;
    public string tabContentName;
    private Image image;

    public void OnPointerDown(PointerEventData eventData)
    {
        print(tabContentName);
        spawnMenu.EnableContent(tabContentName);
        image.color = Color.black;
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
