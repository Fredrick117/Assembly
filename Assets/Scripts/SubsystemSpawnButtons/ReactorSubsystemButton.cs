using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ReactorSubsystemButton : MonoBehaviour
{
    public TMP_Text descriptor;
    public TMP_Text description;
    public ReactorSubsystem data;

    private SubsystemSelectionMenu menuRef;

    public void OnPointerClick(PointerEventData eventData)
    {
        menuRef.AddSubsystemToSlot(data);
        menuRef.gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        menuRef = transform.parent.transform.parent.GetComponent<SubsystemSelectionMenu>();

        descriptor = transform.GetChild(1).transform.GetChild(0).GetComponent<TMP_Text>();
        description = transform.GetChild(1).transform.GetChild(1).GetComponent<TMP_Text>();

        descriptor.text = data.displayName;
        description.text = data.description;
    }
}
