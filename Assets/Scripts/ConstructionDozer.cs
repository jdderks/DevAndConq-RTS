using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructionDozer : Unit
{

    [Header("Construction dozer related"), SerializeField] private PanelInfoScriptableObject constructWarFactoryPanelInfo;
    [SerializeField] private PanelInfoScriptableObject constructTurretActionPanelInfo;
    private List<RtsAction> bulldozerActions = new();
    ConstructBuildingRTSAction constructWarFactoryAction = new();
    ConstructBuildingRTSAction constructTurretAction = new();


    public ConstructBuildingRTSAction ConstructWarFactoryAction { get => constructWarFactoryAction; private set => constructWarFactoryAction = value; }
    public ConstructBuildingRTSAction ConstructTurretAction { get => constructTurretAction; set => constructTurretAction = value; }

    private void Start()
    {
        StartTask(new IdleTask(this));
        ConstructWarFactoryAction.SetActionValues(this);
        ConstructWarFactoryAction.PanelInfo = constructWarFactoryPanelInfo;//GameManager.Instance.Settings.rtsActionSettings.constructWarFactoryPanelInfo;

        ConstructTurretAction.SetActionValues(this);
        ConstructTurretAction.PanelInfo = constructTurretActionPanelInfo;
        bulldozerActions.Add(ConstructWarFactoryAction);
        bulldozerActions.Add(ConstructTurretAction);
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
