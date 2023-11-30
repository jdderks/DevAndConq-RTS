using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class CreateUnitRTSAction : RtsAction
{
    public GameObject unitGameObject; //The unit gameobject that can be instantiated
    public Building building;

    private Transform spawnPointOrigin = null;

    PanelInfoScriptableObject panelInfo; //The info that can be displayed

    public void SetUnitValues(GameObject unit, Building building, Team team)
    {
        this.unitGameObject = unit;
        this.building = building;

        this.spawnPointOrigin = building.unitSpawnPoint;
    }



    //Create the unit
    public override void Activate()
    {
        Assert.IsNotNull(unitGameObject, "Use SetUnitValues to set the values before activating the RTSAction!");
        Assert.IsNotNull(spawnPointOrigin, "Use SetUnitValues to set the values before activating the RTSAction!");

        var unitToSpawn = GameObject.Instantiate(unitGameObject, spawnPointOrigin);
        var unit = unitToSpawn.GetComponent<Unit>();
        unit.SetTeam(building.ownedByTeam.teamStyle);
    }

    //This could be used in the future to get the origin of the action
    public override ISelectable GetOrigin()
    {
        throw new System.NotImplementedException();
    }
}
