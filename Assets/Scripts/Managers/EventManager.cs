using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance { get; private set; }

    public delegate void SubmitDesign();
    public static SubmitDesign onSubmit;

    [SerializeField]
    private GameObject NoShipClassText;

    private Coroutine errorTextCoroutine;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public void OnSubmitClicked()
    {
        if (ShipStats.Instance.currentClass == ShipClassification.None)
        {
            if (errorTextCoroutine != null)
            {
                StopCoroutine(errorTextCoroutine);
            }

            NoShipClassText.SetActive(true);
            errorTextCoroutine = StartCoroutine(HideNoShipText());

            return;
        }

        onSubmit?.Invoke();
    }

    private IEnumerator HideNoShipText()
    {
        yield return new WaitForSeconds(5f);
        NoShipClassText.SetActive(false);
        errorTextCoroutine = null;
    }
}
