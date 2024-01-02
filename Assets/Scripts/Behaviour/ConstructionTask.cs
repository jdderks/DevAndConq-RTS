using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructionTask : SequenceTask
{
    Building targetBuilding;
    
    public override void OnBegin()
    {
        //Distance check from building, only do this task if unit is close enough to the building.

        if (Vector3.Distance(agent.gameObject.transform.position, targetBuilding.gameObject.transform.position) < 10f)
        {

        }
        else
        {
            Cancel();
        }
    }

    public override void OnCancel()
    {
        throw new System.NotImplementedException();
    }

    public override void OnComplete()
    {
        throw new System.NotImplementedException();
    }
}
