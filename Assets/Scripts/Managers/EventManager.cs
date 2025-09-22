using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static event Action OnSubmit;
    public static event Action OnClear;

    public void OnSubmitClicked()
    {
        print("On submit clicked");
        OnSubmit?.Invoke();
    }

    public void OnClearClicked()
    {
        OnClear?.Invoke();
    }
}
