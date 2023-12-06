using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class CreateUnitRTSAction : RtsBuildingAction
{
    public Building building; //Origin building
    public GameObject unitGameObject; //The unit gameobject that can be instantiated
    private PanelInfoScriptableObject panelInfo; //The info that can be displayed

    private Transform spawnPointOrigin = null;

    public PanelInfoScriptableObject PanelInfo { get => panelInfo; set => panelInfo = value; }

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
        return building;
    }

    public override PanelInfoScriptableObject GetPanelInfo()
    {
        return panelInfo;
    }
}
