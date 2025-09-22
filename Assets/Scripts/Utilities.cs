using System;
using UnityEngine;

public class Utilities
{
    public static T GetRandomEnumValue<T>() where T : Enum
    {
        Array enumValues = Enum.GetValues(typeof(T));
        int randomIndex = UnityEngine.Random.Range(0, enumValues.Length);
        return (T)enumValues.GetValue(randomIndex);
    }

    public static bool IsChild(GameObject parent, GameObject child)
    {
        return child.transform.IsChildOf(parent.transform);
    }

    public static ConnectorDirection GetOppositeDirection(ConnectorDirection direction)
    {
        if (direction == ConnectorDirection.East)
            return ConnectorDirection.West;
        if (direction == ConnectorDirection.West)
            return ConnectorDirection.East;
        if (direction == ConnectorDirection.North)
            return ConnectorDirection.South;
        
        return ConnectorDirection.North;
    }

    public static bool IsRootModule(GameObject module)
    {
        if (ShipManager.Instance.rootModule == null || ShipManager.Instance.rootModule == module)
        {
            return true;
        }

        return false;
    }
}
