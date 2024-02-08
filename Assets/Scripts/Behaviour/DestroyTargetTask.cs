using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyTargetTask : UnitTask
{

    private IDamageable target;

    public DestroyTargetTask(Unit agent, IDamageable target)
    {
        this.unit = agent;
        this.target = target;
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
