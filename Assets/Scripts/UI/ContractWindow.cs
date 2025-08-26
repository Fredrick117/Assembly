using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ContractWindow : MonoBehaviour
{
    [SerializeField]
    private TMP_Text customerInfoText;
    [SerializeField]
    private TMP_Text shipInfoText;
    public void CloseWindow()
    {
        gameObject.SetActive(false);
    }

    public void PopulateInfoText()
    {

    }
}
