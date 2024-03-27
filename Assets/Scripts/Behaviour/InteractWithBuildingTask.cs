using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractWithBuildingTask : UnitTask
{

    private Building building;

    public InteractWithBuildingTask(Unit unit, Building building)
    {
        this.unit = unit;
        this.building = building;
    }

    public override void OnBegin()
    {
        if (Vector3.Distance(unit.transform.position, building.unitSpawnPoint.position) < 3)
            InteractWithBuilding();
        else
            Cancel();
    }

    public override void OnCancelled()
    {
        
    }

    public override void OnComplete()
    {
        
    }

    public void InteractWithBuilding()
    {
        if (building.UnitInteract(unit))
            Complete();
        else
            Cancel();
        
    }
}
