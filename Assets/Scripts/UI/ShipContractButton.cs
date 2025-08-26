using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipContractButton : MonoBehaviour
{
    private ContractWindow contractWindow;
    public Canvas canvas;

    private void Awake()
    {
        contractWindow 
            = canvas.transform.Find("Panel_ContractWindow").gameObject.GetComponent<ContractWindow>();
    }

    public void OnClick()
    {
        OpenContractWindow();
    }

    private void OpenContractWindow()
    {
        contractWindow.PopulateInfoText();
        contractWindow.gameObject.SetActive(true);
    }
}
