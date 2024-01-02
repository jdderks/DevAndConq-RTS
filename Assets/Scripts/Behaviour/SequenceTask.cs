using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequenceTask : UnitTask
{
    int currentTaskIndex = -1;
    private List<UnitTask> subTasks = new List<UnitTask>();

    public void AddTask(UnitTask task)
    {
        subTasks.Add(task);
    }

    public override void OnBegin()
    {
        currentTaskIndex = -1;
        StartNextTask();
    }

    public override void OnCancel()
    {
        if (currentTaskIndex >= 0 && currentTaskIndex < subTasks.Count)
        {
            subTasks[currentTaskIndex].OnCancel();
            // Remove subscription from last event if any
            if (currentTaskIndex > 0)
            {
                subTasks[currentTaskIndex - 1].Completed -= OnSubTaskCompleted;
            }
        }
    }

    public override void OnComplete()
    {
        // All tasks completed
    }

    private void StartNextTask()
    {
        currentTaskIndex++;

        if (currentTaskIndex < subTasks.Count)
        {
            subTasks[currentTaskIndex].OnBegin();
            subTasks[currentTaskIndex].Completed += OnSubTaskCompleted;
        }
        else
        {
            Complete();
        }
    }

    private void OnSubTaskCompleted()
    {
        subTasks[currentTaskIndex].Completed -= OnSubTaskCompleted;
        StartNextTask();
    }
}
