using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugManager : MonoBehaviour
{
    [SerializeField] private PathfindingTester pathfindingTester = null;
    [SerializeField] private SelectableCollection selection = null;


    private void Update()
    {

    }


    [Button("Give selected units movement sequenceTask")]
    private void GiveSelectedUnitsIndefiniteMovementTask()
    {
        foreach (Unit unit in selection.GetSelectedUnits())
        {
            var sequence = GetSequenceTask(unit);
            sequence.IsRepeated = true;
            unit.StartTask(sequence);
        }
    }


    public UnitTask GetSequenceTask(Unit unit)
    {
        var sequenceTask = new RepeatingSequenceTask(unit, shouldRepeat: true);
        for (int i = 0; i < pathfindingTester.Transforms.Count; i++)
        {
            var movementTask = new MoveUnitTask(unit, pathfindingTester.Transforms[i].position);
            sequenceTask.AddTask(movementTask);
        }
        return sequenceTask;
    }
}
