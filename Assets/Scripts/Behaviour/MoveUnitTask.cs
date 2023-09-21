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
        agent.Agent.SetDestination(destination);
    }
    public override void OnComplete()
    {
        agent.IsMoving = false;
    }

    public override void OnCancel()
    {
        agent.IsMoving = false;
    }

    /// <summary>
    /// This is the movement logic, called in an update loop.
    /// </summary>
    public void Move()
    {
        if (agent.Agent.remainingDistance < 1)
        {
            Complete();
        }
    }


    private void RotateToWaypoint(Vector3 wayPoint, float rotationSpeed)
    {
        Vector3 directionToTarget = (wayPoint - agent.transform.position).normalized;

        // Create a rotation that points the forward direction towards the target
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget, agent.transform.up);

        // Smoothly interpolate towards the target rotation
        agent.transform.rotation = Quaternion.Slerp(agent.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    
    
    }

    private void CalculatePath(Vector3 target)
    {
        destination = target;
        NavMesh.CalculatePath(agent.transform.position, destination, NavMesh.AllAreas, Path);

    }
}
