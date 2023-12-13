using System.Collections.Generic;
using UnityEngine;

public class ActionQueue
{
    private List<RtsQueueAction> items = new();
    private bool isPaused = false;
    private bool isProcessingQueue = false;

    private bool isOutDated = false;

    public List<RtsQueueAction> Items { get => items; private set => items = value; }
    public bool IsOutDated { get => isOutDated; set => isOutDated = value; }

    public void Update()
    {
        if (items.Count > 0)
        {
            if (!isProcessingQueue)
            {
                StartProcessingQueue();
            }
            else
            {
                ProcessQueue();
            }
        }
    }

    public void SetActions()
    {
        Items = new();
    }

    private void StartProcessingQueue()
    {
        isProcessingQueue = true;
        RtsQueueAction currentAction = items[0];
        currentAction.RemainingTime = currentAction.WaitForSeconds;
        IsOutDated = true;
    }

    private void ProcessQueue()
    {
        RtsQueueAction currentAction = items[0];
        if (!isPaused)
        {
            currentAction.RemainingTime -= Time.deltaTime;
        }

        if (currentAction.RemainingTime <= 0)
        {
            currentAction.Action.Activate();
            RemoveFromQueue(0);
            isProcessingQueue = false;
        }
    }

    public void AddToActionQueue(RtsAction action)
    {
        if (items.Count < 8)
        {
            items.Add(new RtsQueueAction(action, action.GetPanelInfo().constructionTime));
            IsOutDated = true;
        }
    }

    public void RemoveFromQueue(int i = 0)
    {
        if (i < items.Count)
        {
            if (i == 0 && isProcessingQueue)
            {
                isProcessingQueue = false;
            }
            items.RemoveAt(i);
        }
        IsOutDated = true;
    }

    public void PauseResumeQueue(bool pause)
    {
        isPaused = pause;
    }

    public int GetQueueContentAmount()
    {
        return items.Count;
    }
}
