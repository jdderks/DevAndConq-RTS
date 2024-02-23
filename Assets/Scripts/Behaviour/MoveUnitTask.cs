using UnityEngine;
using UnityEngine.AI;

public class MoveUnitTask : UnitTask
{
    protected Vector3 destination;
    private NavMeshPath path;
    public NavMeshPath Path { get => path; set => path = value; }

    public MoveUnitTask(Unit agent, Vector3 destination)
    {
        this.unit = agent;
        this.destination = destination;

        Priority = TaskPriority.Busy;

        agent.IsMoving = true;
        Path = new NavMeshPath();
    }

    public override void OnBegin()
    {
        unit.IsMoving = true;
        unit.MoveToPoint(destination);//Agent.SetDestination(destination);
    }

    public override void OnComplete()
    {
        //Debug.Log(unit + " finished moving!");
        unit.IsMoving = false;
    }

    public override void OnCancelled()
    {
        unit.IsMoving = false;
    }
}
