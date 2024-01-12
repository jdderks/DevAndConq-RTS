using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPositioner : MonoBehaviour
{
    [SerializeField] public List<BuildingPosition> buildingPositions = new();

    [Button("Get Building Positions from children objects")]
    public void FillBuildingPositionsWithChildrenObjects()
    {
        buildingPositions = new List<BuildingPosition>();

        // For each child gameobject
        foreach (Transform child in transform)
        {
            // Create a new BuildingPosition
            BuildingPosition newBuildingPosition = new BuildingPosition();

            // Give the new buildingPosition the Vector3 position from the child object
            newBuildingPosition.position = child.position;

            // Add the new BuildingPosition to the list
            buildingPositions.Add(newBuildingPosition);
        }
    }
}

[System.Serializable]
public struct BuildingPosition
{
    public Vector3 position;
    public bool occupied;
}