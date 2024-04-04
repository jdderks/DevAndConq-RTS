using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;

public class ChaseTask : RepeatingSequenceTask
{


    public ChaseTask(Unit agent, bool shouldRepeat = false) : base(agent, shouldRepeat)
    {
        
    }


}
