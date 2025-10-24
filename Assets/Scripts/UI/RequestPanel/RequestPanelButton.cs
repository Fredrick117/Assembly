using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class RequestPanelButton : MonoBehaviour, IPointerClickHandler
{
    public GameObject requestPanel;
    public TMP_Text buttonArrow;

    public void OnPointerClick(PointerEventData eventData)
    {
        requestPanel.SetActive(!requestPanel.activeSelf);

        if (requestPanel.activeSelf)
            buttonArrow.text = "<";
        else
            buttonArrow.text = ">";
    }
}
