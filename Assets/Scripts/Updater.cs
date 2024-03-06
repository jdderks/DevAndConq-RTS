using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using NaughtyAttributes;

public class Updater : MonoBehaviour
{
    [SerializeField] private List<IUpdateable> updateables = new();

    void Update()
    {
        OnUpdate();
    }

    private void OnUpdate()
    {
        for (int i = 0; i < updateables.Count; i++)
        {
            updateables[i].OnUpdate(Time.deltaTime);
        }
    }

    public void RegisterUpdateable(IUpdateable updateable)
    {
        if (!updateables.Contains(updateable))
        {
            updateables.Add(updateable);
        }
    }

    public void UnregisterUpdateable(IUpdateable updateable)
    {
        if (updateables.Contains(updateable))
        {
            updateables.Remove(updateable);
        }
    }
}
