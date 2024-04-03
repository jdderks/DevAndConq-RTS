using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AttackTask : UnitTask
{

    IDamageable target;

    public AttackTask(IDamageable target)
    {
        this.target = target;
    }

    public override void OnBegin()
    {
        if (Vector3.Distance(unit.transform.position, target.GetGameObject().transform.position) < unit.AttackRange)
        {
            unit.RangedAttack(unit, target);
        } 
        else
        {
            Cancel();
        }
    }

    public override void OnCancelled()
    {
        unit.StopAttacking();
    }

    public override void OnComplete()
    {
        unit.StopAttacking();
    }
}
