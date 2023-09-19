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

public enum TaskPriority
{
    Idle = 0,
    Priority = 1
}

public abstract class UnitTask
{
    protected Unit agent;

    public TaskPriority priority;

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
        if (Begun == null)
        {
            Begun += OnBegin;
            Begun.Invoke();
        }
    }

    public void Cancel()
    {
        this.TaskState = TaskState.Cancelled;
        if (Canceled == null)
        {
            Canceled += OnCancel;
            Canceled.Invoke();
        }
    }

    public void Complete()
    {
        this.TaskState = TaskState.Completed;
        if (Completed == null)
        {
            Completed += OnComplete;
            Completed.Invoke();
        }
    }
}
