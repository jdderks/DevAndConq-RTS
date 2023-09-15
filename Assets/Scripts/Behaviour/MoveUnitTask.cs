using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveUnitTask : UnitTask
{
    protected Vector3 destination;
    private NavMeshPath path;

    public NavMeshPath Path { get => path; set => path = value; }

    public MoveUnitTask(Unit agent,Vector3 destination)
    {
        this.agent = agent;
        this.destination = destination;

        agent.IsMoving = true;
        Path = new NavMeshPath();
    }

    public override void OnBegin()
    {

    }
    public override void OnComplete()
    {
        agent.IsMoving = false;
    }

    public override void OnCancel()
    {
        agent.IsMoving = false;
    }

    public void Move()
    {

    }

    private void CalculatePath(Vector3 target)
    {
        destination = target;
        NavMesh.CalculatePath(agent.transform.position, destination, NavMesh.AllAreas, Path);

    }
}
