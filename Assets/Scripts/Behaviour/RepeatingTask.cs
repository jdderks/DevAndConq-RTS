using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

/// <summary>
/// For making tasks repeated
/// </summary>
public class RepeatingTask : UnitTask
{


    UnitTask repeatingTask;

    /// <summary>
    /// Make a task repeated, note that you can repeat Sequence tasks as well.
    /// </summary>
    /// <param name="repeatingTask"></param>
    public RepeatingTask(UnitTask repeatingTask)
    {
        this.repeatingTask = repeatingTask;
    }

    public override void OnBegin()
    {
        repeatingTask.Completed += StartTask;
        repeatingTask.Canceled += OnTaskCancelled;
        StartTask();
    }

    public override void OnCancelled()
    {
        repeatingTask.Cancel();
        UnsubscribeTasks();
    }

    public override void OnComplete()
    {
    }

    public void StartTask()
    {
        repeatingTask.Begin();
    }

    public void OnTaskCancelled()
    {
        repeatingTask.Cancel();
    }

    private void UnsubscribeTasks()
    {
        repeatingTask.Completed -= StartTask;
        repeatingTask.Canceled -= OnTaskCancelled;

    }
}
