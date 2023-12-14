using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CommandCenter : Building
{
    CreateUnitRTSAction constructDozerAction = new CreateUnitRTSAction();
    CreateUnitRTSAction constructLightTankAction = new CreateUnitRTSAction();

    //ActionQueue ActionQueue = new();
    
    private void Start()
    {
        var dozerUnitInfo = GameManager.Instance.Settings.rtsActionSettings.bullDozerPanelInfo;
        var lightTankUnitInfo = GameManager.Instance.Settings.rtsActionSettings.lightTankPanelInfo;

        constructDozerAction.PanelInfo = dozerUnitInfo;
        constructLightTankAction.PanelInfo = lightTankUnitInfo;
        
        GetActions().Add(constructDozerAction);
        GetActions().Add(constructLightTankAction);

        constructDozerAction.SetUnitValues(
            unitObject: dozerUnitInfo.actionPrefab,
            originBuilding: this,
            team: ownedByTeam);

        constructLightTankAction.SetUnitValues(
            unitObject: lightTankUnitInfo.actionPrefab,
            originBuilding: this,
            team: ownedByTeam);
    }


    public override GameObject GetGameObject()
    {
        return gameObject;
    }

    public override void Select()
    {
        Assert.IsNotNull(selectableHighlightParent, "Parent object not set in prefab.");
        instantiatedSelectionObject = Instantiate(
            GameManager.Instance.selectionManager.SelectionPrefab,
            selectableHighlightParent
            );



        //var selectableCollection = GameManager.Instance.SelectableCollection;
        //var uimanager = GameManager.Instance.uiManager;

        //uimanager.UpdateRtsActionPanel(units: selectableCollection.GetSelectedUnits(), building: this);
    }

    public override void Deselect()
    {
        Destroy(instantiatedSelectionObject);
    }

}
