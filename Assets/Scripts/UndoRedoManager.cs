using System.Collections.Generic;
using UnityEngine;

public class UndoRedoManager
{
    private Stack<NoiseValues> undoStack = new Stack<NoiseValues>();
    private Stack<NoiseValues> redoStack = new Stack<NoiseValues>();

    public void StorePreviousState(NoiseValues values)
    {
        // Clone the current state
        NoiseValues clonedValues = values.Clone();

        // Push the cloned state onto the undoStack
        undoStack.Push(clonedValues);

        // Clear redoStack
        redoStack.Clear();
    }

    public bool CanUndo()
    {
        return undoStack.Count > 0;
    }

    public bool CanRedo()
    {
        return redoStack.Count > 0;
    }

    public NoiseValues Undo(NoiseValues currentValue)
    {
        if (CanUndo())
        {
            NoiseValues previousValues = undoStack.Pop();
            redoStack.Push(currentValue);
            return previousValues;
        }
        else
        {
            return currentValue;
        }
    }

    public NoiseValues Redo(NoiseValues currentValue)
    {
        if (CanRedo())
        {
            NoiseValues nextValues = redoStack.Pop();
            undoStack.Push(currentValue);
            return nextValues;
        }
        else
        {
            return currentValue;
        }
    }
}