using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionQueueItem : MonoBehaviour
{
    private RtsAction actionToExecute;

    public Image backImage; //Often black & white / monochrome
    public Image frontImage; //Foreground image that is filled over time

    private float waitDurationSeconds = 5f;

    public float WaitDurationSeconds { get => waitDurationSeconds; private set => waitDurationSeconds = value; }
    public RtsAction ActionToExecute { get => actionToExecute; private set => actionToExecute = value; }

    public ActionQueueItem(RtsAction action,float waitDurationSeconds)
    {
        this.ActionToExecute = action;
        this.WaitDurationSeconds = waitDurationSeconds;
    }
}
