using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SubsystemSelectionMenu : MonoBehaviour
{
    public int selectedSlot;

    public void SetSelectedSlot(int index)
    {
        selectedSlot = index;
    }
}
