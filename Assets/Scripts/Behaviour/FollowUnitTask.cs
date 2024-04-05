using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowUnitTask : UnitTask
{
    GameObject followTarget;

    public FollowUnitTask(GameObject followTarget)
    {
        this.followTarget = followTarget;
    }



    public override void OnBegin()
    {
        Debug.Log(unit + " is following " + followTarget.ToString());

        unit.MovementTarget = followTarget;
        unit.CurrentMoveState = MovementState.Following;
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
