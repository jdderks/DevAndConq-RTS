using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleTask : UnitTask
{
    public IdleTask(Unit agent)
    {
        this.unit = agent;
    }

    public override void OnBegin()
    {
        //Assert.IsNotNull(agent.IdleAnimation, "you have to assign an idle animation to the unit!");
        //if (agent.IdleAnimation != null)
        //    agent.IdleAnimation.Play();
        Priority = TaskPriority.Idle;
        unit.PlayIdleAnimation();
    }

    public override void OnCancelled()
    {
        //throw new System.NotImplementedException();
        unit.StopIdleAnimation();
    }

    public override void OnComplete()
    {
        unit.StopIdleAnimation();
        //throw new System.NotImplementedException();
    }
}
