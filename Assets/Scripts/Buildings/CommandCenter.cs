using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CommandCenter : Building
{
    CreateUnitRTSAction constructDozerAction = new CreateUnitRTSAction();

    private void Start()
    {
        constructDozerAction.PanelInfo = GameManager.Instance.Settings.rtsActionSettings.bullDozerPanelInfo;
        rtsBuildingActions.Add(constructDozerAction);

        constructDozerAction.SetUnitValues(
            UnitToSpawn.LightTank,
            this,
            ownedByTeam);
    }


    public override GameObject GetGameObject()
    {
        return gameObject;
    }

    public override void Select()
    {
        Assert.IsNotNull(selectableHighlightParent, "Parent object not set in prefab.");
        instantiatedSelectionObject = Instantiate(GameManager.Instance.Settings.modelSettings.unitSelectionHighlightGameObject, selectableHighlightParent);



        //var selectableCollection = GameManager.Instance.SelectableCollection;
        //var uimanager = GameManager.Instance.uiManager;

        //uimanager.UpdateRtsActionPanel(units: selectableCollection.GetSelectedUnits(), building: this);
    }

    public override void Deselect()
    {
        Destroy(instantiatedSelectionObject);
    }

}
