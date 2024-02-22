using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Experimental.AI;

public class SupplyTruck : Unit
{
    private List<RtsAction> supplyTruckActions = new();
    private SupplyDock currentSupplyDock;

    private void Start()
    {
        currentSupplyDock = SetCurrentSupplyDock();
        if ()
        {
            var sequenceTask = new SequenceTask(this);

            //SEt necessary tasks
            var moveTask = new MoveUnitTask(this, currentSupplyDock.);

            var repeatingTask = new RepeatingTask(sequenceTask);
            StartTask(repeatingTask);
        }
        StartTask(new IdleTask(this));
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

    public void SetCurrentSupplyDock()
    {
        var gameObjectsInProximity = GetUnitsAndBuildingsInProximity();
        SupplyDock supplydock = gameObjectsInProximity.FirstOrDefault(x => x.GetComponent<SupplyDock>() != null).GetComponent<SupplyDock>();

        if (supplydock != null)
        {
            currentSupplyDock = supplydock;
        }
    }
}
