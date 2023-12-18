using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    private Unit originUnit;
    private GameObject buildingToPlace;

    private bool isBuilding;

    public void EnterBuildingPlacementMode(Unit originUnit, GameObject buildingToPlace)
    {
        this.originUnit = originUnit;
        this.buildingToPlace = buildingToPlace;

        isBuilding = true;
    }

    public void Update()
    {
        if (isBuilding)
        {
            ConstructBuilding();
        }
    }

    public void ConstructBuilding()
    {
        if (isBuilding)
        {
            if (Input.GetMouseButtonDown(0)) // Left mouse button to place object
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.CompareTag("Terrain"))
                    {
                        var buildingGameObject = Instantiate(buildingToPlace, hit.point, Quaternion.identity);
                        var building = buildingGameObject.GetComponent<Building>();
                        
                        building.ConstructionPercentage = 0;

                        isBuilding = false;
                        originUnit = null;
                        buildingToPlace = null;
                    }
                }
            }
        }
    }






    //This class puts the player into a "constructing" state where they can place buildings

    //This state is triggered through the "ConstructBuildingRTSAction"

    //The player can at any point quit the "constructing" state by either constructing something with left click
    // or cancelling with right click.

    //Being in the "Constructing State" communicates with other managers 
}
