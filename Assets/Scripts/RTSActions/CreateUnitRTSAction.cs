using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;

public class CreateUnitRTSAction : RtsBuildingAction
{
    public Building building; //Origin building
    private Team team;
    private GameObject unitGameObject; //The unit gameobject that can be instantiated
    private PanelInfoScriptableObject panelInfo; //The info that can be displayed

    private Transform spawnPointOrigin = null;

    public PanelInfoScriptableObject PanelInfo { get => panelInfo; set => panelInfo = value; }

    public void SetUnitValues(GameObject unitObject, Building originBuilding, Team team)
    {
        this.unitGameObject = unitObject;
        this.building = originBuilding;
        this.spawnPointOrigin = originBuilding.unitSpawnPoint;
        this.team = team;
    }


    public override GameObject Activate()
    {
        Assert.IsNotNull(unitGameObject, "Use SetUnitValues to set the values before activating the RTSAction!");
        Assert.IsNotNull(spawnPointOrigin, "Use SetUnitValues to set the values before activating the RTSAction!");
        
        var economy = GameManager.Instance.economyManager.GetEconomy(building.ownedByTeam.teamByColour);
        //Debug.Log("Activated effect with cost: " + GetPanelInfo().cost);

        bool couldAfford = economy.CanAffordAction(this);
        if (!couldAfford)
        {
            Debug.Log("Unit not constructed! Not enough money");
            return null;
        }

        economy.DecreaseMoney(GetPanelInfo().cost);

        var unitToSpawn = GameObject.Instantiate(unitGameObject, spawnPointOrigin.position, Quaternion.identity);

        var unit = unitToSpawn.GetComponent<Unit>();
        unit.SetTeam(building.ownedByTeam.teamByColour);




        return unitToSpawn;

        //unitToSpawn.tag = building.gameObject.tag;
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
