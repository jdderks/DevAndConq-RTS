using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructBuildingRTSAction : RtsAction
{
    private Unit unit;

    private GameObject buildingPrefab;

    private PanelInfoScriptableObject panelInfo; //The info that can be displayed
    public PanelInfoScriptableObject PanelInfo { get => panelInfo; set => panelInfo = value; }

    public void SetActionValues(Unit unit)
    {
        this.unit = unit;
    }


    public override void Activate()
    {
        SequenceTask constructBuildingSequenceTask = new();
        constructBuildingSequenceTask.AddTask(new MoveUnitTask(unit, buildingPrefab.transform.position));
        throw new System.NotImplementedException();
    }

    public override ISelectable GetOrigin()
    {
        return unit;
    }

    public override PanelInfoScriptableObject GetPanelInfo()
    {
        Assert.IsNotNull(PanelInfo, "the panelInfo of this action has not been set yet!");
        return PanelInfo;
    }

    public override RTSActionType GetActionType()
    {
        return PanelInfo.actionType;
    }
}
