using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NoShipSelectedText : MonoBehaviour
{
    private TMP_Text text;

    void Start()
    {
        text = GetComponent<TMP_Text>();
    }

    public void HideText()
    {
        gameObject.SetActive(false);
    }

    public void ShowText()
    {
        gameObject.SetActive(true);
    }
}
