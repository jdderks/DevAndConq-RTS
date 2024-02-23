using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatherSuppliesTask : UnitTask
{
    SupplyDock supplyDock;
    bool hasSupplies = false;
    int amountOfSupplies = 0;


    public GatherSuppliesTask(Unit unit, SupplyDock supplyDock)
    {
        this.unit = unit;
        this.supplyDock = supplyDock;
    }

    public override void OnBegin()
    {
        GatherSupplies();
    }

    public override void OnCancelled()
    {

    }

    public override void OnComplete()
    {

    }

    private void GatherSupplies()
    {
        supplyDock.RemoveSupplies(gatheringUnit: unit);

    }

}
