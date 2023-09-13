using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitTask
{

    public Action Completed;
    public Action Canceled;
    public Action Begun;

    public abstract void OnBegin();

    public abstract void OnCancel();

    public abstract void OnComplete();


    public void Begin()
    {
        OnBegin();
    }

    public void Cancel()
    {
        OnCancel();
    }

    public void Complete()
    {
        OnComplete();
    }

}
