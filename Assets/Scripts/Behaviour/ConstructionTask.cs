using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructionTask : UnitTask
{
    Building targetBuilding;
    Unit unit;

    public ConstructionTask(Unit agent, Building targetBuilding)
    {
        this.unit = agent;
        this.targetBuilding = targetBuilding;
    }

    public override void OnBegin()
    {
        targetBuilding.StartConstruction(unit, 1);
    }

    public override void OnCancelled()
    {
        targetBuilding.StopConstruction();
    }

    public override void OnComplete()
    {
    }

    public void Finish()
    {
        Complete();
    }
}
