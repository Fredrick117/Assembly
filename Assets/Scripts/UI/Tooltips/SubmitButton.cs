using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class SubmitButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject feedbackTooltip;
    private bool isHovering = false;

    private RectTransform tooltipRect;
    private TMP_Text feedbackText;

    private void Start()
    {
        ShipStats.Instance.onStatsChanged.AddListener(SetTooltipMessage);
        tooltipRect = feedbackTooltip.GetComponent<RectTransform>();
        feedbackText = feedbackTooltip.transform.GetChild(0).gameObject.GetComponent<TMP_Text>();
    }

    private void OnDisable()
    {
        ShipStats.Instance.onStatsChanged.RemoveListener(SetTooltipMessage);
    }

    private void Update()
    {
        if (isHovering)
        {
            Vector2 mousePos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                tooltipRect.parent as RectTransform,
                Input.mousePosition,
                null,
                out mousePos
            );

            tooltipRect.anchoredPosition = mousePos - new Vector2(tooltipRect.sizeDelta.x / 2, -tooltipRect.sizeDelta.y / 2);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        print("enter!");
        isHovering = true;
        feedbackTooltip.SetActive(true);
        SetTooltipMessage();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        print("exit!");
        isHovering = false;
        feedbackTooltip.SetActive(false);
    }

    private void SetTooltipMessage()
    {
        if (feedbackTooltip != null)
        {
            if (ShipStats.Instance.baseStats == null)
            {
                feedbackText.text = "No ship has been selected!";
                return;
            }

            feedbackText.text = " ";

            RequestData request = ShipRequestManager.Instance.activeShipRequest;
            ShipStats shipStats = ShipStats.Instance;

            if (shipStats.Speed < request.minSpeed)
            {
                feedbackText.text += "This ship is too slow!\n";
            }
            if (!shipStats.baseStats.shipClass.Equals(request.shipClass))
            {
                feedbackText.text += "This ship is not the right class!\n";
            }
            if (shipStats.Armor < request.minArmor)
            {
                feedbackText.text += "This ship doesn't have enough armor!\n";
            }
            if (shipStats.MaxPower < request.minPower)
            {
                feedbackText.text += "This ship doesn't produce enough power!\n";
            }
        }
    }
}
