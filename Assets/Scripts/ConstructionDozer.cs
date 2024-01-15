using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructionDozer : Unit
{

    [Header("Construction dozer related"), SerializeField] private PanelInfoScriptableObject constructWarFactoryPanelInfo;
    private List<RtsAction> bulldozerActions = new();
    ConstructBuildingRTSAction constructWarFactoryAction = new();


    public ConstructBuildingRTSAction ConstructWarFactoryAction { get => constructWarFactoryAction; private set => constructWarFactoryAction = value; }

    private void Start()
    {
        StartTask(new IdleTask(this));
        ConstructWarFactoryAction.SetActionValues(this);
        ConstructWarFactoryAction.PanelInfo = constructWarFactoryPanelInfo;//GameManager.Instance.Settings.rtsActionSettings.constructWarFactoryPanelInfo; 
        bulldozerActions.Add(ConstructWarFactoryAction);
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
