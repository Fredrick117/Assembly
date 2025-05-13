using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipClassSelector : MonoBehaviour
{
    [SerializeField]
    private SubsystemListPanel subsystemListPanel;

    public void OnSelectShipClass(ShipBaseStats shipStats)
    {
        ShipManager.Instance.SetShipBaseStats(shipStats);
        subsystemListPanel.UpdateSubsystemSlots();
        gameObject.SetActive(false);
    }
}
