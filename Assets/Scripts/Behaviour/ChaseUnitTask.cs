using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;

/// <summary>
/// Chasing means moving in range of a target and attacking it when in range.
/// </summary>
public class ChaseUnitTask : UnitTask
{
    GameObject chaseTarget;

    public ChaseUnitTask(Unit agent, GameObject target)
    {
        unit = agent;
        chaseTarget = target;
    }

    public override void OnBegin()
    {
        Debug.Log(unit + " is chasing " + chaseTarget.ToString());

        unit.MovementTarget = chaseTarget;
        unit.currentMovementState = MovementState.Chasing;
    }

    public override void OnCancelled()
    {
        unit.ResetTargetAndMoveState();
    }


    public override void OnComplete()
    {
        unit.ResetTargetAndMoveState();
    }
}
