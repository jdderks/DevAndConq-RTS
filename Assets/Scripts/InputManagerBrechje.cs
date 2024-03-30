using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InputManagerBrechje : MonoBehaviour
{
    [SerializeField] private KeyCode holdToInput;



    [SerializeField] private KeyCode undoInput;
    [SerializeField] private KeyCode redoInput;

    [SerializeField] public Action undoAction;
    [SerializeField] public Action redoAction;

    public void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        //if (!Input.GetKeyDown(holdToInput)) return;

        if (Input.GetKeyDown(undoInput))
        {
            undoAction?.Invoke();
        }
        if (Input.GetKeyDown(redoInput))
        {
            redoAction?.Invoke();
        }
    }
}
