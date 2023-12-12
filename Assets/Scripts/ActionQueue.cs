using System.Collections.Generic;
using UnityEngine;

public class ActionQueue : MonoBehaviour
{
    private List<ActionQueueItem> actions = new List<ActionQueueItem>();
    private bool isPaused = false;
    private bool isProcessingQueue = false;
    private float remainingTime = 0f;

    public List<ActionQueueItem> Actions { get => actions; private set => actions = value; }

    private void Update()
    {
        if (actions.Count > 0)
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
        Actions = new();
    }

    private void StartProcessingQueue()
    {
        isProcessingQueue = true;
        ActionQueueItem currentAction = actions[0];
        remainingTime = currentAction.WaitDurationSeconds;
    }

    private void ProcessQueue()
    {
        if (!isPaused)
        {
            remainingTime -= Time.deltaTime;
            Debug.Log("Remaining Time" + remainingTime);
        }

        if (remainingTime <= 0)
        {
            ActionQueueItem currentAction = actions[0];
            currentAction.ActionToExecute.Activate();
            actions.RemoveAt(0);
            isProcessingQueue = false;
        }
    }

    public void AddToActionQueue(ActionQueueItem queueAction)
    {
        actions.Add(queueAction);
    }

    public void RemoveFromQueue(int i = 0)
    {
        if (i < actions.Count)
        {
            if (i == 0 && isProcessingQueue)
            {
                isProcessingQueue = false;
            }
            actions.RemoveAt(i);
        }
    }

    public void PauseResumeQueue(bool pause)
    {
        isPaused = pause;
    }

    public int GetQueueContentAmount()
    {
        return actions.Count;
    }
}
