using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTask : UnitTask
{
    Turret turret;
    IDamageable target;

    public FireTask(Turret turret, IDamageable target)
    {
        this.turret = turret;
        this.target = target;
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
