using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupplyCenter : Building
{
    CreateUnitRTSAction constructSupplyTruck = new CreateUnitRTSAction();
    [SerializeField] private PanelInfoScriptableObject supplyTruckUnitInfo;// = GameManager.Instance.Settings.rtsActionSettings.lightTankPanelInfo;



    public void Start()
    {
        if (!isInstantiated) SetTeam(teamByColour);
        constructSupplyTruck.PanelInfo = supplyTruckUnitInfo;

        GetActions().Add(constructSupplyTruck);

        constructSupplyTruck.SetUnitValues(
            unitObject: supplyTruckUnitInfo.actionPrefab,
            originBuilding: this,
            team: ownedByTeam);
    }

    public override void Deselect()
    {
        throw new System.NotImplementedException();
    }

    public override GameObject GetGameObject()
    {
        throw new System.NotImplementedException();
    }

    public override void Select()
    {
        throw new System.NotImplementedException();
    }
}
