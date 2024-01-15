using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequenceTask : UnitTask
{
    private List<UnitTask> subtasks = new List<UnitTask>();
    private int currentTaskIndex = 0;

    public SequenceTask(Unit agent, params UnitTask[] tasks)
    {
        this.unit = agent;
        subtasks.AddRange(tasks);
    }

    public override void OnBegin()
    {
        if (subtasks.Count > 0)
        {
            ExecuteNextTask();
        }
        else
        {
            Complete();
        }
    }

    public override void OnCancelled()
    {
        if (currentTaskIndex < subtasks.Count)
        {
            subtasks[currentTaskIndex].Cancel();
        }
    }

    public override void OnComplete()
    {
        // SequenceTask completed
    }

    private void ExecuteNextTask()
    {
        if (currentTaskIndex < subtasks.Count)
        {
            UnitTask nextTask = subtasks[currentTaskIndex];
            nextTask.Completed += OnSubtaskCompleted;
            nextTask.Begin();
        }
        else
        {
            Complete();
        }
    }

    private void OnSubtaskCompleted()
    {
        UnitTask completedTask = subtasks[currentTaskIndex];
        completedTask.Completed -= OnSubtaskCompleted;

        currentTaskIndex++;
        ExecuteNextTask();
    }

    public UnitTask GetCurrentTask()
    {
        if (currentTaskIndex >= 0 && currentTaskIndex < subtasks.Count)
        {
            return subtasks[currentTaskIndex];
        }
        else
        {
            Debug.LogWarning("No current task available in the sequence.");
            return null;
        }
    }
}
