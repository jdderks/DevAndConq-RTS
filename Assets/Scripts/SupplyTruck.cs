using System;
using UnityEngine.Assertions;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Experimental.AI;

public class SupplyTruck : Unit
{
    private List<RtsAction> supplyTruckActions = new();
    private SupplyDock currentSupplyDock;
    private SupplyCenter currentSupplyCenter;

    private void OnEnable()
    {
        SetCurrentSupplyDockAndCenter();
        if (currentSupplyDock != null)
        {
            //Set repeating sequence task to constantly go to the supply dock and back.
            var moveToSupplyDockTask = new MoveUnitTask(this, currentSupplyDock.InteractPosition.position);
            var harvestTask = new GatherSuppliesTask(this, currentSupplyDock);
            var moveToSupplyCenterTask = new MoveUnitTask(this, currentSupplyCenter.InteractPosition.position);
            var deliverTask = new DeliverSuppliesTask(this, currentSupplyCenter);

            var sequenceTask = new SequenceTask(this);
            sequenceTask.AddTask(moveToSupplyDockTask);
            sequenceTask.AddTask(harvestTask);
            sequenceTask.AddTask(moveToSupplyCenterTask);
            sequenceTask.AddTask(deliverTask);

            var repeatingTask = new RepeatingTask(sequenceTask);

            Assert.IsNotNull(currentSupplyDock, "supply dock is null!");
            Assert.IsNotNull(currentSupplyCenter, "supply center is null!");

            StartTask(repeatingTask);
        }
        else
        {
            StartTask(new IdleTask(this));
        }
    }

    public override void Die()
    {
        base.Die();
    }

    public override void TakeDamage(float amount)
    {
        Health -= amount;
    }

    public override List<RtsAction> GetActions()
    {
        return supplyTruckActions;
    }

    public void SetCurrentSupplyDockAndCenter()
    {
        currentSupplyCenter = GetCloseBySupplyCenter();
        SupplyDock supplydock = GetCloseBySupplyDock();

        if (supplydock != null)
        {
            currentSupplyDock = supplydock;
        }
    }

    private SupplyCenter GetCloseBySupplyCenter()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 30);
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.layer == LayerMask.NameToLayer("Building"))
            {
                if (collider.gameObject == gameObject) continue; //don't add itself to the list
                var supplyCenter = collider.gameObject.GetComponent<SupplyCenter>();
                if (supplyCenter != null)
                    return supplyCenter;
            }
        }
        return null;
    }

    public SupplyDock GetCloseBySupplyDock()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 30);
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.layer == LayerMask.NameToLayer("SupplyDock"))
            {
                if (collider.gameObject == gameObject) continue; //don't add itself to the list
                var supplyDock = collider.gameObject.GetComponent<SupplyDock>();
                if (supplyDock != null)
                    return supplyDock;
               
            }
        }
        return null;
    }
}
