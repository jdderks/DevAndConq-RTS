using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RTSTimer : IUpdateable
{
    private Updater updater;

    public event Action TimeElapsed; // Event to be invoked when time has elapsed

    private bool isRunning = false;
    private float elapsedTime = 0f;
    private float duration = 0f;

    public RTSTimer(float duration)
    {
        this.duration = duration;
        updater = GameManager.Instance.updater;
        updater.RegisterUpdateable(this);
    }


    // Start the timer with the specified duration in seconds
    public void StartTimer(float seconds)
    {
        if (!isRunning)
        {
            duration = seconds;
            elapsedTime = 0f;
            isRunning = true;
        }
    }

    // Update the timer
    public void UpdateTimer(float deltaTime)
    {
        if (isRunning)
        {
            elapsedTime += deltaTime;

            // Check if time has elapsed
            if (elapsedTime >= duration)
            {
                isRunning = false;
                OnTimeElapsed();
            }
        }
    }

    // Invokes the TimeElapsed event
    private void OnTimeElapsed()
    {
        TimeElapsed?.Invoke();
    }

    // Reset the timer
    public void ResetTimer()
    {
        elapsedTime = 0f;
        isRunning = false;
    }


    public void Destroy()
    {
        updater.UnregisterUpdateable(this);
    }
    public void OnUpdate(float dt)
    {
        UpdateTimer(dt);
    }
}
