using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewShipRequestData", menuName = "Requests/RequestData")]
public class ShipRequestData : ScriptableObject
{
    public string requestMessage;
    public ShipRole shipRole;
    public List<ShipClassification> desiredClasses;
    public ShipData enemyShipData;
}
