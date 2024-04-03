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
            //currentAction.Action.Activate();
            currentAction.OnActivate?.Invoke();
            RemoveFromQueue(0);
            isProcessingQueue = false;
        }
    }

    public RtsQueueAction AddToActionQueue(RtsAction action, List<Unit> listToAddTo = null)
    {
        if (items.Count > 8) return null; //TODO: magic number

        var queueAction = new RtsQueueAction(action, action.GetPanelInfo().actionDelay);
        items.Add(queueAction);
        IsOutDated = true;

        if (listToAddTo != null)
        {
            queueAction.OnActivate += () =>
            {
                var unit = action.Activate().GetComponent<Unit>();
                listToAddTo.Add(unit);
            };
        }
        else
        {
            queueAction.OnActivate += () => action.Activate();
        }
        return queueAction;
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
