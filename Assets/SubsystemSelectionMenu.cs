using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.UI;
using static Unity.PlasticSCM.Editor.WebApi.CredentialsResponse;

public class SubsystemSelectionMenu : MonoBehaviour
{
    public SubsystemSlot selectedSlot;
    
    public TextAsset jsonFile;

    public GameObject subsystemPrefab;

    public Transform subsystemContainer;

    void Start()
    {
        
    }

    public void SetSelectedSlot(SubsystemSlot slot)
    {
        selectedSlot = slot;
    }

    public void AddSubsystemToSlot(Subsystem subsystem)
    {
        ShipBase ship = ShipManager.Instance.currentShip.GetComponent<ShipBase>();

        if (subsystem is Reactor reactorData)
        {
            ship.ModifyPower(reactorData.powerOutput);
        }

        if (subsystem is Shielding shieldData)
        {
            ship.ModifyShielding(shieldData.shieldStrength);
        }

        selectedSlot.SetSubsystem(subsystem);

        ShipManager.Instance.currentShip.GetComponent<ShipBase>().subsystems.Add(subsystem);
    }
}
