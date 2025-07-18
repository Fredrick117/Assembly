using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipClassSelectionMenu : MonoBehaviour
{
    [SerializeField]
    private SubsystemListPanel subsystemListPanel;

    public Canvas mainCanvas;

    private NoShipSelectedText noShipText;

    private void Start()
    {
        noShipText = mainCanvas.GetComponentInChildren<NoShipSelectedText>();
    }

    public void OnSelectShipClass(ShipBaseStats shipStats)
    {
        ShipManager.Instance.SetShipBaseStats(shipStats);
        
        subsystemListPanel.UpdateSubsystemSlots();
        
        gameObject.SetActive(false);

        if (noShipText.gameObject.activeSelf)
            noShipText.HideText();
    }
}
