using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrayHelpers : MonoBehaviour
{
    public static T GetRandomElement<T>(IEnumerable<T> list)
    {
        List<T> listCopy = new List<T>(list);
        if (listCopy.Count == 0)
        {
            Debug.LogWarning("No elements available.");
            return default;
        }

        int randomIndex = UnityEngine.Random.Range(0, listCopy.Count);
        return listCopy[randomIndex];
    }
}
