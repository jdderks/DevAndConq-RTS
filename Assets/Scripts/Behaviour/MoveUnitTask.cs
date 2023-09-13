using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveUnitTask : UnitTask
{
    private Vector3 destination;


    public MoveUnitTask(Vector3 destination)
    {
        this.destination = destination;
    }


    public override void OnBegin()
    {

    }
    public override void OnComplete()
    {

    }

    public override void OnCancel()
    {

    }
}
