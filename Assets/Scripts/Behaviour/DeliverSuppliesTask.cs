using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliverSuppliesTask : UnitTask
{
    SupplyCenter supplyCenterToDeliverTo;

    public DeliverSuppliesTask(SupplyTruck truck, SupplyCenter supplyCenterToDeliverTo)
    {
        this.unit = truck;
        this.supplyCenterToDeliverTo = supplyCenterToDeliverTo;
    }
    public override void OnBegin()
    {

    }

    public override void OnCancelled()
    {

    }

    public override void OnComplete()
    {

    }
}
