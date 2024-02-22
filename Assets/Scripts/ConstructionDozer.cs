using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructionDozer : Unit
{

    [Header("Construction dozer related")]
    [SerializeField] private PanelInfoScriptableObject constructWarFactoryPanelInfo;
    [SerializeField] private PanelInfoScriptableObject constructTurretActionPanelInfo;
    [SerializeField] private PanelInfoScriptableObject constructSupplyCenterActionPanelInfo;

    private List<RtsAction> bulldozerActions = new();

    ConstructBuildingRTSAction constructWarFactoryAction = new();
    ConstructBuildingRTSAction constructTurretAction = new();
    ConstructBuildingRTSAction constructSupplyCenter = new();


    public ConstructBuildingRTSAction ConstructWarFactoryAction { get => constructWarFactoryAction; private set => constructWarFactoryAction = value; }
    public ConstructBuildingRTSAction ConstructTurretAction { get => constructTurretAction; set => constructTurretAction = value; }
    public ConstructBuildingRTSAction ConstructSupplyCenter { get => constructSupplyCenter; set => constructSupplyCenter = value; }

    private void Start()
    {
        StartTask(new IdleTask(this));
        ConstructWarFactoryAction.SetActionValues(this);
        ConstructWarFactoryAction.PanelInfo = constructWarFactoryPanelInfo;

        ConstructTurretAction.SetActionValues(this);
        ConstructTurretAction.PanelInfo = constructTurretActionPanelInfo;

        ConstructSupplyCenter.SetActionValues(this);
        ConstructSupplyCenter.PanelInfo = constructSupplyCenterActionPanelInfo;

        bulldozerActions.Add(ConstructWarFactoryAction);
        bulldozerActions.Add(ConstructTurretAction);
        bulldozerActions.Add(ConstructSupplyCenter);
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
