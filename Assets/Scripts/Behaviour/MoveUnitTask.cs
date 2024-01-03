using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class MoveUnitTask : UnitTask
{
    protected Vector3 destination;
    private NavMeshPath path;
    private int currentCornerIndex = 0;

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
        //CalculatePath(destination);
        Debug.Log(agent);
        Debug.Log(agent.Agent);
        agent.IsMoving = true;
        agent.Agent.SetDestination(destination);
    }
    public override void OnComplete()
    {
        Debug.Log("Finished moving!");
        agent.IsMoving = false;
    }

    public override void OnCancelled()
    {
        agent.IsMoving = false;
    }
}
