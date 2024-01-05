using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class MoveUnitTask : UnitTask
{
    protected Vector3 destination;
    private NavMeshPath path;

    public NavMeshPath Path { get => path; set => path = value; }

    public MoveUnitTask(Unit agent, Vector3 destination)
    {
        priority = TaskPriority.Priority;
        this.agent = agent;
        this.destination = destination;

        agent.IsMoving = true;
        Path = new NavMeshPath();
    }

    public override void OnBegin()
    {
        agent.IsMoving = true;
        agent.Agent.SetDestination(destination);
    }

    public override void OnComplete()
    {
        Debug.Log(agent + " finished moving!");
        agent.IsMoving = false;
    }

    public override void OnCancelled()
    {
        agent.IsMoving = false;
    }
}
