using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructionTask : UnitTask
{
    Building targetBuilding;
    bool ignoreDistance = false;

    public ConstructionTask(Unit agent, Building targetBuilding, bool ignoreDistance = false)
    {
        this.unit = agent;
        this.targetBuilding = targetBuilding;
        this.ignoreDistance = ignoreDistance;
    }

    public override void OnBegin()
    {
        if (ignoreDistance || Vector3.Distance(unit.transform.position, targetBuilding.transform.position) < 15)
            targetBuilding.StartConstruction(unit, 1);
        else
        {
            Complete();
            Debug.Log("No construction due to distance");
        }


    }

    public override void OnCancelled()
    {
        targetBuilding.StopConstruction();
    }

    public override void OnComplete()
    {
        //targetBuilding.SetTeam(unit.OwnedByTeam.teamByColour);
    }

    public void Finish()
    {
        Complete();
    }
}
