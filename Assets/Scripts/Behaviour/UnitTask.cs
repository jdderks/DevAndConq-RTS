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
    Busy = 1
}

public abstract class UnitTask
{
    protected Unit unit;

    private TaskPriority priority;

    public Action Completed;
    public Action Canceled;
    public Action Begun;

    public abstract void OnBegin();

    public abstract void OnCancelled();

    public abstract void OnComplete();

    private TaskState taskState;

    public TaskState TaskState { get => taskState; set => taskState = value; }
    public TaskPriority Priority { get => priority; set => priority = value; }

    public void Begin()
    {
        this.TaskState = TaskState.Active;
        OnBegin();
        if (taskState == TaskState.Active)
            Begun?.Invoke();

    }

    public void Cancel()
    {
        this.TaskState = TaskState.Cancelled;
        OnCancelled();
        Canceled?.Invoke();
    }

    public void Complete()
    {
        this.TaskState = TaskState.Completed;
        OnComplete();
        Completed?.Invoke();

    }
}
