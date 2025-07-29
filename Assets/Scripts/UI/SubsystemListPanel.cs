using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SubsystemListPanel : MonoBehaviour
{
    public Sprite imageSprite;

    public GameObject subsystemMenu;

    public GameObject slotPrefab;

    public List<GameObject> slots = new List<GameObject>();

    public void UpdateSubsystemSlots()
    {
        if (ShipManager.Instance.currentShip.GetComponent<ShipStats>().baseStats == null)
        {
            print("UpdateSubsystemSlots: base stats is null!");
            ClearSlots();
            return;
        }

        ClearSlots();

        int numSlots = ShipManager.Instance.currentShip.GetComponent<ShipStats>().baseStats.utilitySlots;

        for (int i = 0; i < numSlots; i++)
        {
            GameObject slot = GameObject.Instantiate(slotPrefab, transform);
            slot.GetComponent<SubsystemSlot>().slotIndex = i;

            slots.Add(slot);
        }
    }

    private void ClearSlots()
    {
        for (int i = 0; i < transform.childCount; i++) 
        {
            GameObject.Destroy(transform.GetChild(i).gameObject);
        }

        slots.Clear();

        ShipStats.Instance.subsystems.Clear();
    }
}
