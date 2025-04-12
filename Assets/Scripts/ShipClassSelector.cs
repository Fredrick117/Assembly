using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipClassSelector : MonoBehaviour
{
    public void OnSelectShipClass(BaseShipStats shipStats)
    {
        ShipManager.Instance.SetShipStats(shipStats);

        gameObject.SetActive(false);
    }
}
