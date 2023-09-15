using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TaskState
{
    Inactive = 0,
    Active = 1,
    Cancelled = 2,
    Completed = 3
}

public abstract class UnitTask
{

    protected Unit agent;

    public Action Completed;
    public Action Canceled;
    public Action Begun;

    public abstract void OnBegin();

    public abstract void OnCancel();

    public abstract void OnComplete();

    private TaskState taskState;

    public TaskState TaskState { get => taskState; set => taskState = value; }

    public void Begin()
    {
        this.TaskState = TaskState.Active;
        OnBegin();
    }

    public void Cancel()
    {
        this.TaskState = TaskState.Cancelled;
        OnCancel();
    }

    public void Complete()
    {
        this.TaskState = TaskState.Completed;
        OnComplete();
    }
}
