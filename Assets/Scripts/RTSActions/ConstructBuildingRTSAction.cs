using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructBuildingRTSAction : RtsAction
{
    private GameObject buildingPrefab;

    private PanelInfoScriptableObject panelInfo; //The info that can be displayed
    public PanelInfoScriptableObject PanelInfo { get => panelInfo; set => panelInfo = value; }

    public override void Activate()
    {
        throw new System.NotImplementedException();
    }

    public override ISelectable GetOrigin()
    {
        throw new System.NotImplementedException();
    }

    public override PanelInfoScriptableObject GetPanelInfo()
    {
        throw new System.NotImplementedException();
    }
}
