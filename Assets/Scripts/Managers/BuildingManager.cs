using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//This class was written with some help from this tutorial, I really only skimmed through it but it felt right to still credit it: https://www.youtube.com/watch?v=YuGabUUSqlE

//This class puts the player into a "constructing" state where they can place buildings

//This state is triggered through the "ConstructBuildingRTSAction"

//The player can at any point quit the "constructing" state by either constructing something with left click
// or cancelling with right click.

//Being in the "Constructing State" communicates with other managers 



public class BuildingManager : Manager
{
    [SerializeField] private LayerMask terrainLayerMask;

    private Unit originUnit;
    private GameObject buildingToPlace;

    private GameObject ghostObject = null;
    [SerializeField] private GameObject ghostObjectPrefab = null;

    private bool isBuilding;

    Vector3 posToPlace = Vector3.zero;

    List<Building> allBuildings = new();

    public void EnterBuildingPlacementMode(Unit originUnit, GameObject buildingToPlace)
    {
        if (originUnit != null)
            this.originUnit = originUnit;
        //Debug.Log(originUnit.OwnedByTeam.teamByColour);

        this.buildingToPlace = buildingToPlace;
        Building building = buildingToPlace.GetComponent<Building>();
        building.ResetConstruction();
        ghostObject = Instantiate(ghostObjectPrefab, posToPlace, Quaternion.identity);

        isBuilding = true;
    }

    public void Update()
    {
        if (isBuilding)
        {
            ConstructingBuilding();
        }
    }

    public void ConstructingBuilding()
    {
        if (isBuilding)
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, Mathf.Infinity, terrainLayerMask))
            {
                posToPlace = hit.point;
                ghostObject.transform.position = posToPlace;

                if (Input.GetMouseButtonDown(0)) // Left mouse button to place object
                {
                    GameObject buildingGameObject = Instantiate(buildingToPlace, hit.point, Quaternion.identity);
                    Building building = buildingGameObject.GetComponent<Building>();

                    building.SetTeam(originUnit.OwnedByTeam.teamByColour);
                    building.ResetConstruction();// = 0;

                    MoveUnitTask moveTask = new(originUnit, building.unitSpawnPoint.position);
                    ConstructionTask constructionTask = new(originUnit, building);

                    // Move and then construct the building
                    var sequenceConstructionTask = new SequenceTask(originUnit, moveTask, constructionTask);

                    originUnit.StartTask(sequenceConstructionTask);

                    Destroy(ghostObject);
                    isBuilding = false;
                    originUnit = null;
                    buildingToPlace = null;
                }
            }

        }
    }


}