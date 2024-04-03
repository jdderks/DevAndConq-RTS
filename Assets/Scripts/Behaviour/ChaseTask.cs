using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;

public class ChaseTask : RepeatingSequenceTask
{
    Vector3 targetPosition;
    IDamageable target;



    public void SetTarget(Vector3 targetPosition, IDamageable target)
    {
        this.targetPosition = targetPosition;
        this.target = target;
    }

    public ChaseTask(Unit agent, bool shouldRepeat = false) : base(agent, shouldRepeat)
    {
        Assert.IsNotNull(target, "don't forget to set the values!");
        Subtasks.Add(new MoveUnitTask(unit, targetPosition));
        Subtasks.Add(new AttackTask(target));
    }

    
}
