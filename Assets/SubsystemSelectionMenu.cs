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
        //selectedSlot.SetSubsystem(subsystem);

        //ShipManager.Instance.currentShip.GetComponent<ShipBase>().subsystems.Add(subsystem);

        //print(subsystem.displayName);
    }
}
