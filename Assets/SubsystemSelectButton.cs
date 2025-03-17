using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class SubsystemSelectButton : MonoBehaviour, IPointerClickHandler
{
    public TMP_Text descriptor;
    public TMP_Text description;
    public Subsystem subsystemData;

    private SubsystemSelectionMenu menuRef;

    public void OnPointerClick(PointerEventData eventData)
    {
        menuRef.AddSubsystemToSlot(subsystemData);
        menuRef.gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        // TODO: move this to SubsystemSelectionMenu so that Subsystem fields can be populated automatically
        subsystemData = new ReactorSubsystem();
        subsystemData.displayName = "Mk. V Fusion Reactor";
        subsystemData.description = "A state-of-the-art reactor that utilizes nuclear fusion to output massive amounts of power.";

        descriptor.text = subsystemData.displayName;
        description.text = subsystemData.description;

        menuRef = transform.parent.transform.parent.GetComponent<SubsystemSelectionMenu>(); // TODO ?
    }
}
