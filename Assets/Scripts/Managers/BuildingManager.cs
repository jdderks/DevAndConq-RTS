using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class was written with some help from this tutorial, I really only skimmed through it but it felt right to still credit it: https://www.youtube.com/watch?v=YuGabUUSqlE

//This class puts the player into a "constructing" state where they can place buildings

//This state is triggered through the "ConstructBuildingRTSAction"

//The player can at any point quit the "constructing" state by either constructing something with left click
// or cancelling with right click.

//Being in the "Constructing State" communicates with other managers 



public class BuildingManager : MonoBehaviour
{
    private Unit originUnit;
    private GameObject buildingToPlace;

    private GameObject ghostObject = null;
    [SerializeField] private GameObject ghostObjectPrefab = null;

    private bool isBuilding;

    Vector3 posToPlace = Vector3.zero;

    public void EnterBuildingPlacementMode(Unit originUnit, GameObject buildingToPlace)
    {
        this.originUnit = originUnit;
        this.buildingToPlace = buildingToPlace;

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
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag("Terrain"))
                {
                    posToPlace = hit.point;
                    ghostObject.transform.position = posToPlace;

                    if (Input.GetMouseButtonDown(0)) // Left mouse button to place object
                    {
                        GameObject buildingGameObject = Instantiate(buildingToPlace, hit.point, Quaternion.identity);
                        Building building = buildingGameObject.GetComponent<Building>();

                        building.SetAsConstructing();// = 0;

                        var sequenceConstructionTask = new SequenceTask();

                        MoveUnitTask moveTask = new(originUnit, buildingGameObject.transform.position + new Vector3(3,0,0));
                        ConstructionTask constructionTask = new(originUnit, building);

                        sequenceConstructionTask.AddTask(moveTask);
                        sequenceConstructionTask.AddTask(constructionTask);

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


}
