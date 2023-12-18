using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

//public enum UnitToSpawn
//{
//    None = 0,
//    Bulldozer = 1,
//    LightTank = 2,
//    HeavyTank = 3
//}

public class CreateUnitRTSAction : RtsBuildingAction
{
    public Building building; //Origin building

    private GameObject unitGameObject; //The unit gameobject that can be instantiated
    private PanelInfoScriptableObject panelInfo; //The info that can be displayed

    private Transform spawnPointOrigin = null;

    public PanelInfoScriptableObject PanelInfo { get => panelInfo; set => panelInfo = value; }

    public void SetUnitValues(GameObject unitObject, Building originBuilding, Team team)
    {
        this.unitGameObject = unitObject;
        this.building = originBuilding;
        this.spawnPointOrigin = originBuilding.unitSpawnPoint;
    }


    public override void Activate()
    {
        Assert.IsNotNull(unitGameObject, "Use SetUnitValues to set the values before activating the RTSAction!");
        Assert.IsNotNull(spawnPointOrigin, "Use SetUnitValues to set the values before activating the RTSAction!");

        var unitToSpawn = GameObject.Instantiate(unitGameObject, spawnPointOrigin.position, Quaternion.identity);
        var unit = unitToSpawn.GetComponent<Unit>();
        //unit.SetTeam(building.ownedByTeam.teamStyle);
    }

    //This could be used in the future to get the origin of the action
    public override ISelectable GetOrigin()
    {
        return building;
    }

    public Building GetBuilding()
    {
        return building;
    }

    public override PanelInfoScriptableObject GetPanelInfo()
    {
        return panelInfo;
    }

    public override RTSActionType GetActionType()
    {
        return PanelInfo.actionType;
    }
}
