using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateUnitRTSAction : RTSAction
{
    [SerializeField] public Unit unit; //The unit that can be made
    [SerializeField] PanelInfoScriptableObject panelInfo; //The info that can be displayed

    //Create the unit
    public override void Activate()
    {
        throw new System.NotImplementedException();
    }

    //This could be used in the future to get the origin of the action
    public override ISelectable GetOrigin()
    {
        throw new System.NotImplementedException();
    }
}
