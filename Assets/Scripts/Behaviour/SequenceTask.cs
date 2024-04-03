using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequenceTask : UnitTask
{
    private List<UnitTask> subtasks = new List<UnitTask>();
    protected int currentTaskIndex = 0;

    public List<UnitTask> Subtasks { get => subtasks; set => subtasks = value; }

    public SequenceTask(Unit agent/*, params UnitTask[] tasks*/)
    {
        this.unit = agent;
        //subtasks.AddRange(tasks);
    }

    public override void OnBegin()
    {
        if (Subtasks.Count > 0)
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
        if (currentTaskIndex < Subtasks.Count)
        {
            Subtasks[currentTaskIndex].Cancel();
        }
    }

    public override void OnComplete()
    {
        Debug.Log("Sequence Task completed.");
    }

    public void AddTask(UnitTask task, bool addAsNext = false)
    {
        if (addAsNext)
        {
            Subtasks.Insert(currentTaskIndex, task);
        }
        else
        {
            Subtasks.Add(task);
        }
    }

    public void AddTaskAndRunImmediately(UnitTask task)
    {
        var currentRunningTask = Subtasks[currentTaskIndex];
        Subtasks.Insert(currentTaskIndex + 1, task);
        task.Completed += ExecuteNextTask;
    }




    protected void ExecuteNextTask()
    {
        if (currentTaskIndex < Subtasks.Count)
        {
            UnitTask nextTask = Subtasks[currentTaskIndex];
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
        UnitTask completedTask = Subtasks[currentTaskIndex];
        completedTask.Completed -= OnSubtaskCompleted;

        currentTaskIndex++;
        ExecuteNextTask();
    }

    public UnitTask GetCurrentTask()
    {
        if (currentTaskIndex >= 0 && currentTaskIndex < Subtasks.Count)
        {
            return Subtasks[currentTaskIndex];
        }
        else
        {
            Debug.LogWarning("No current task available in the sequence.");
            return null;
        }
    }
}
