using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using UnityEngine;

/// <summary>
/// For making tasks repeated
/// </summary>
public class RepeatingTask : UnitTask
{
    UnitTask repeatingTask;
    RTSTimer retryTimer;

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

    public void StartTask()
    {
        repeatingTask.Begin();
    }

    public void OnTaskCancelled()
    {
        retryTimer = new RTSTimer(1f);
        retryTimer.TimeElapsed += OnRetryTimer;
        //repeatingTask.Cancel();
    }

    private void UnsubscribeTasks()
    {
        retryTimer.Destroy();
        repeatingTask.Completed -= StartTask;
        repeatingTask.Canceled -= OnTaskCancelled;
    }

    public void OnRetryTimer()
    {
        retryTimer.TimeElapsed -= OnRetryTimer;
        retryTimer = null;
        StartTask();
    }

    public override void OnComplete(){}
}
