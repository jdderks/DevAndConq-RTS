using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionQueue : MonoBehaviour
{
    private List<ActionQueueItem> actions = new();

    public void StartQueue()
    {
        //Check if there is actionqueueitems in the list
    }

    public void StopQueue(bool clearQueue = false)
    {
        if (clearQueue)
        {
            actions.Clear();
            //Stop the queue
        }
        else
        {
            //Just stop the queue, and if startqueue is called it will continue where it left off
        }
    }

    public void AddToActionQueue(RtsAction action)
    {

    }

    public void RemoveFromQueue(int i = 0)
    {

    }

    public void UpdateActionQueue()
    {

    }
}
