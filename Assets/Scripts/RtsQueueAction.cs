using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RtsQueueAction
{
    private RtsAction action;
    private float waitForSeconds;

    private float remainingTime;
    public float RemainingTime { get => remainingTime; set => remainingTime = value; }
    public float WaitForSeconds { get => waitForSeconds; private set => waitForSeconds = value; }
    public RtsAction Action { get => action; set => action = value; }

    public Action OnActivate;

    public RtsQueueAction(RtsAction action, float waitForSeconds)
    {
        RemainingTime = waitForSeconds;
        this.Action = action;
        this.WaitForSeconds = waitForSeconds;
    }

}
