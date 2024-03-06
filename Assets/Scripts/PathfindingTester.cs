using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingTester : MonoBehaviour
{
    [SerializeField, ReadOnly] private List<Transform> transforms = new();

    public List<Transform> Transforms { get => transforms; set => transforms = value; }

    [Button("Add all children to list.")]
    public void AddAllChildren()
    {
        Transforms.Clear();
        foreach (Transform child in transform)
        {
            Transforms.Add(child);
        }
    }
}
