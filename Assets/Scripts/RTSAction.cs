using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RtsAction
{
    public abstract void Activate();

    public abstract ISelectable GetOrigin();

    public abstract PanelInfoScriptableObject GetPanelInfo();

    public abstract RTSActionType GetActionType();
}

public abstract class RtsBuildingAction : RtsAction
{

}

public abstract class RtsUnitAction : RtsAction
{
    
}