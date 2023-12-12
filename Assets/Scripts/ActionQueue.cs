using System.Collections.Generic;
using UnityEngine;

public class ActionQueue
{
    private List<RtsQueueAction> items = new();
    private bool isPaused = false;
    private bool isProcessingQueue = false;

    public List<RtsQueueAction> Items { get => items; private set => items = value; }

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
    }

    private void ProcessQueue()
    {
        RtsQueueAction currentAction = items[0];
        if (!isPaused)
        {

            currentAction.RemainingTime -= Time.deltaTime;
            Debug.Log("Remaining Time" + currentAction.RemainingTime);
        }

        if (currentAction.RemainingTime <= 0)
        {
            currentAction.Action.Activate();
            items.RemoveAt(0);
            isProcessingQueue = false;
        }
    }

    public void AddToActionQueue(RtsAction action)
    {
        if (items.Count < 8)
        {
            items.Add(new RtsQueueAction(action, action.GetPanelInfo().constructionTime));

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
