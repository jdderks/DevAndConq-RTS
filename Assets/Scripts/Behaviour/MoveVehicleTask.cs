using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveVehicleTask : MoveUnitTask
{
    [SerializeField] public float moveForce = 10.0f; // The force applied to move the vehicle
    [SerializeField] public float maxSpeed = 10.0f; // The maximum speed of the vehicle
    [SerializeField] public float turnForce = 5.0f; // The force applied to steer the vehicle

    [SerializeField] public float easingThreshold = 5f;

    private NavMeshPath path;
    private int currentWaypoint = 0;
    private Rigidbody rb;

    private Vector3 direction;


    public MoveVehicleTask(Unit agent, Vector3 destination) : base(agent ,destination)
    {
        rb = agent.GetGameObject().GetComponent<Rigidbody>();
        path = new NavMeshPath();
    }

    public override void OnBegin()
    {
        base.OnBegin();
    }

    public override void OnCancelled()
    {
        base.OnCancelled();
    }

    public override void OnComplete()
    {
        base.OnComplete();
    }
}
