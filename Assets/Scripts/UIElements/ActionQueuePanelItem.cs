using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionQueuePanelItem : MonoBehaviour
{
    private RtsAction actionToExecute;

    public Image backImage; //Often black & white / monochrome
    public Image frontImage; //Foreground image that is filled over time

    private float waitDurationSeconds = 5f;
    private float remainingTime;

    public float WaitDurationSeconds { get => waitDurationSeconds; private set => waitDurationSeconds = value; }
    public RtsAction ActionToExecute { get => actionToExecute; private set => actionToExecute = value; }
    public float RemainingTime { get => remainingTime; set => remainingTime = value; }

    public void SetValues(RtsAction action,float waitDurationSeconds)
    {
        this.ActionToExecute = action;
        this.WaitDurationSeconds = waitDurationSeconds;
    }
}
