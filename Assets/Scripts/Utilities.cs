using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Utilities
{
    /// <summary>
    /// Returns a random value of the specified enum type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">An enum type.</typeparam>
    /// <returns>
    /// A randomly selected value from the set of values defined by the enum <typeparamref name="T"/>.
    /// Uses <see cref="UnityEngine.Random.Range(int, int)"/> with an index range of [0, enumValues.Length).
    /// </returns>
    public static T GetRandomEnumValue<T>() where T : Enum
    {
        Array enumValues = Enum.GetValues(typeof(T));
        int randomIndex = UnityEngine.Random.Range(0, enumValues.Length);
        return (T)enumValues.GetValue(randomIndex);
    }

    /// <summary>
    /// Returns a random value of the specified enum type <typeparamref name="T"/>, optionally skipping
    /// the first enum value (commonly used as a "null" or "None" entry).
    /// </summary>
    /// <typeparam name="T">An enum type.</typeparam>
    /// <param name="enumContainsNullValue">
    /// If <c>true</c>, the method will start selecting from index 1, effectively excluding the enum value
    /// at index 0. If <c>false</c>, selection starts at index 0.
    /// </param>
    /// <returns>
    /// A randomly selected enum value from the defined subset. Selection uses
    /// <see cref="UnityEngine.Random.Range(int, int)"/> with an index range of [startingIndex, enumValues.Length).
    /// </returns>
    /// <remarks>
    /// This method assumes that when <paramref name="enumContainsNullValue"/> is <c>true</c>, the enum's
    /// first element (index 0) represents a null/none entry and should be excluded from random selection.
    /// </remarks>
    public static T GetRandomEnumValue<T>(bool enumContainsNullValue) where T : Enum
    {
        int startingIndex = 0;
        if (enumContainsNullValue)
        {
            startingIndex = 1;
        }

        Array enumValues = Enum.GetValues(typeof(T));
        int randomIndex = UnityEngine.Random.Range(startingIndex, enumValues.Length);
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

    public static T GetRandomListElement<T>(List<T> list)
    {
        return list[UnityEngine.Random.Range(0, list.Count)];
    }

    public static string ArmorRatingToString(int armorRating)
    {
        return ((ArmorRating)armorRating).ToString();
    }
}
