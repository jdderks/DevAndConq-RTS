using UnityEngine;

public class RepeatingSequenceTask : SequenceTask
{
    private bool shouldRepeat = false;

    public RepeatingSequenceTask(Unit agent, bool shouldRepeat = false) : base(agent) 
    {
        this.shouldRepeat = shouldRepeat;
    }

    public override void OnComplete()
    {
        Debug.Log("Repeating Sequence Task completed.");

        if (shouldRepeat)
        {
            currentTaskIndex = 0; // Reset the index to start from the beginning
            ExecuteNextTask(); // Start the sequence again
        }
        else
        {
            base.OnComplete(); // Call base OnComplete if not repeating
        }
    }
}