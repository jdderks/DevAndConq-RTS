using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleTask : UnitTask
{

    public IdleTask(Unit agent)
    {
        this.agent = agent;
    }

    public override void OnBegin()
    {
        //Assert.IsNotNull(agent.IdleAnimation, "you have to assign an idle animation to the unit!");
        //if (agent.IdleAnimation != null)
        //    agent.IdleAnimation.Play();
    }

    public override void OnCancel()
    {
        //throw new System.NotImplementedException();
    }

    public override void OnComplete()
    {
        //throw new System.NotImplementedException();
    }
}
