using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipClassSelectionMenu : MonoBehaviour
{
    [SerializeField]
    private SubsystemListPanel subsystemListPanel;

    [SerializeField] private GameObject subsystemGrid;

    public Canvas mainCanvas;

    private NoShipSelectedText noShipText;

    private void Start()
    {
        noShipText = mainCanvas.GetComponentInChildren<NoShipSelectedText>();
    }

    public void OnSelectShipClass(ShipBaseStats shipBaseStats)
    {
        subsystemGrid.SetActive(true);
        GridManager gridManager = subsystemGrid.GetComponent<GridManager>();
        gridManager.GenerateGrid(shipBaseStats.rows, shipBaseStats.columns);
        
        ShipManager.Instance.SetNewShipClass(shipBaseStats);
        
        gameObject.SetActive(false);

        if (noShipText.gameObject.activeSelf)
            noShipText.HideText();
    }
}
