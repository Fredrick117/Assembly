using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SubsystemListPanel : MonoBehaviour
{
    public Sprite imageSprite;

    public GameObject subsystemMenu;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void UpdateSubsystemSlots()
    {
        if (ShipManager.Instance.currentShip == null)
        {
            Debug.LogError("Current ship is null, can't populate subsystem slots");
            return;
        }

        ClearSlots();

        //int numSlots = ShipManager.Instance.currentShip.GetComponent<ShipBase>().subsystemSlots;

        //for (int i = 0; i < numSlots; i++)
        //{
        //    GameObject imageObject = new GameObject("SubsystemSlot_" + i);

        //    imageObject.transform.SetParent(transform, false);

        //    imageObject.AddComponent<RectTransform>();
        //    SubsystemSlot slot = imageObject.AddComponent<SubsystemSlot>();
        //    slot.subsystemMenu = subsystemMenu;

        //    // An empty slot image
        //    Image image = imageObject.AddComponent<Image>();
        //    image.sprite = imageSprite;

        //    GameObject iconObject = new GameObject("Icon");
        //    iconObject.AddComponent<RectTransform>();

        //    Image iconImage = iconObject.AddComponent<Image>();
        //}
    }

    private void ClearSlots()
    {
        for (int i = 0; i < transform.childCount; i++) 
        {
            GameObject.Destroy(transform.GetChild(i).gameObject);
        }
    }
}
