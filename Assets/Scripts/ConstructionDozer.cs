using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructionDozer : Unit
{
    private List<RtsAction> bulldozerActions = new();
    ConstructBuildingRTSAction constructWarFactoryAction = new();

    

    private void Start()
    {
        //constructBuildingAction.PanelInfo = 
        bulldozerActions.Add(constructWarFactoryAction);
    }

    public override void Die()
    {
        base.Die();
    }

    public override void Hit()
    {
        base.Hit();
    }

    public override void PlayIdleAnimation()
    {
        base.PlayIdleAnimation();
    }

    public override void StopIdleAnimation()
    {
        base.StopIdleAnimation();
    }

    public override float TakeDamage()
    {
        return base.TakeDamage();
    }

    public override List<RtsAction> GetActions()
    {
        return bulldozerActions;
    }
}
