using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SupplyTruck : Unit
{
    private List<RtsAction> supplyTruckActions = new();
    private SupplyDock currentSupplyDock;
    private SupplyCenter currentSupplyCenter;

    [SerializeField]
    private GameObject loadedObject;


    private bool hasLoad = false;
    public bool HasLoad
    {
        get => hasLoad;
        set
        {
            loadedObject.SetActive(value);
            hasLoad = value;
        }
    }

    private void Start()
    {
        SetCurrentSupplyDockAndCenter();
        if (currentSupplyDock != null && currentSupplyCenter != null)
        {
            var repeatingTask = new RepeatingSequenceTask(this, shouldRepeat: true);

            var moveToSupplyDockTask = new MoveUnitTask(this, currentSupplyDock.InteractPosition.position);
            var harvestTask = new InteractWithBuildingTask(this, currentSupplyDock);
            var moveToSupplyCenterTask = new MoveUnitTask(this, currentSupplyCenter.InteractPosition.position);
            var deliverTask = new InteractWithBuildingTask(this, currentSupplyCenter);



            repeatingTask.AddTask(moveToSupplyDockTask);
            repeatingTask.AddTask(harvestTask);
            repeatingTask.AddTask(moveToSupplyCenterTask);
            repeatingTask.AddTask(deliverTask);

            StartTask(repeatingTask);
        }
        else
        {
            Debug.Log($"{this.ToString()} their supplydock is: {currentSupplyDock.ToString()} or their supplycenter is: {currentSupplyCenter.ToString()}");
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
