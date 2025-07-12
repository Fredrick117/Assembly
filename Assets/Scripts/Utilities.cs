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

    public static bool FlipCoin()
    {
        int result = UnityEngine.Random.Range(0, 2);

        if (result == 0)
            return false;
        else
            return true;
    }
}
