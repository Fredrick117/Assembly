using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubsystemSelectionMenu : MonoBehaviour
{
    public SubsystemSlot selectedSlot;

    public void AddSubsystemToSlot(Subsystem data)
    {
        selectedSlot.subsystem = data;

        print(data.displayName);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
