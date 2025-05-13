using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class SubsystemButton : MonoBehaviour, IPointerClickHandler
{
    public TMP_Text descriptor;
    public TMP_Text description;
    public TMP_Text statDescription;
    public Subsystem data;

    private SubsystemSelectionMenu menuRef;

    public void OnPointerClick(PointerEventData eventData)
    {
        //menuRef.AddSubsystemToSlot(data);

        ShipStats.Instance.AddSubsystem(menuRef.selectedSlot, data);
        menuRef.gameObject.SetActive(false);
    }

    void Start()
    {
        menuRef = transform.parent.transform.parent.GetComponent<SubsystemSelectionMenu>();

        descriptor = transform.GetChild(1).transform.GetChild(0).GetComponent<TMP_Text>();
        description = transform.GetChild(1).transform.GetChild(1).GetComponent<TMP_Text>();

        descriptor.text = data.displayName;
        description.text = data.description;

        if (data is Reactor reactorData)
            statDescription.text = $"Power output: {reactorData.powerOutput} GW\tPower type: {reactorData.powerType}";
        if (data is Shielding shieldData)
            statDescription.text = $"Shield strength: {shieldData.shieldStrength}/100\tRecharge speed: {shieldData.rechargeSpeed}% per second";
        if (data is LifeSupport lifeSupportData)
            statDescription.text = $"Crew capacity: {lifeSupportData.crewCapacity}";
    }
}
