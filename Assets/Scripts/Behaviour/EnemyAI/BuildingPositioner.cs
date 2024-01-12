using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPositioner : MonoBehaviour
{
    [SerializeField] public List<BuildingPosition> buildingPositions = new(); 
}


public struct BuildingPosition
{
    public Vector3 position;
    public bool occupied;
}