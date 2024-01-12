using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructionDozer : Unit
{
    private List<RtsAction> bulldozerActions = new();
    ConstructBuildingRTSAction constructWarFactoryAction = new();

    [Header("Construction dozer related"), SerializeField] private PanelInfoScriptableObject constructWarFactoryPanelInfo;

    private void Start()
    {
        constructWarFactoryAction.SetActionValues(this);
        constructWarFactoryAction.PanelInfo = constructWarFactoryPanelInfo;//GameManager.Instance.Settings.rtsActionSettings.constructWarFactoryPanelInfo; 
        bulldozerActions.Add(constructWarFactoryAction);
    }

    public override void Die()
    {
        base.Die();
    }


    public override void PlayIdleAnimation()
    {
        base.PlayIdleAnimation();
    }

    public override void StopIdleAnimation()
    {
        base.StopIdleAnimation();
    }

    public override void TakeDamage(float amount)
    {
        Health -= amount;
    }

    public override List<RtsAction> GetActions()
    {
        return bulldozerActions;
    }
}
