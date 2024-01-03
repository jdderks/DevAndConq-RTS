using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructionTask : UnitTask
{
    Building targetBuilding;
    public ConstructionTask(Unit agent, Building targetBuilding)
    {
        this.agent = agent;
        this.targetBuilding = targetBuilding;
    }

    public override void OnBegin()
    {
        
    }

    public override void OnCancelled()
    {
        throw new System.NotImplementedException();
    }

    public override void OnComplete()
    {
        throw new System.NotImplementedException();
    }
}
