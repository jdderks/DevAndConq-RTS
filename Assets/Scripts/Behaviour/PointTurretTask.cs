using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointTurretTask : UnitTask
{
    GameObject turret;
    Vector3 desiredRotation;

    public PointTurretTask(GameObject turret, Vector3 desiredRotation)
    {
        this.turret = turret;
        this.desiredRotation = desiredRotation;
    }

    public override void OnBegin()
    {
        if (!turret)
            Cancel();
        
        
    }

    public override void OnCancelled()
    {
        throw new System.NotImplementedException();
    }

    public override void OnComplete()
    {
        throw new System.NotImplementedException();
    }
}
