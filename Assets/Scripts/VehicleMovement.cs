using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class VehicleMovement : MonoBehaviour
{
    
    //void Update()
    //{
    //    Vector3 currentDirection = transform.forward;
    //    if (currentWaypoint < path.corners.Length)
    //    {
    //        direction = (path.corners[currentWaypoint] - transform.position).normalized;
    //        float angle = Vector3.Angle(currentDirection, direction);

    //        if (angle > 1)
    //        {
    //            RotateTowardsWaypoint(direction);
    //        }
    //        else
    //        {
    //            MoveTowardsWaypoint();
    //        }

    //        if (Vector3.Distance(transform.position, path.corners[currentWaypoint]) < 0.5f)
    //        {
    //            currentWaypoint++;
    //        }
    //    }
    //}

    //void RotateTowardsWaypoint(Vector3 targetDirection)
    //{
    //    if (targetDirection != Vector3.zero)
    //    {
    //        Vector3 currentDirection = transform.forward;
    //        float angle = Vector3.Angle(currentDirection, targetDirection);
    //        Debug.Log(angle);
    //        if (angle > 1)
    //        {
    //            // Calculate the torque axis based on the target direction
    //            Vector3 torqueAxis = Vector3.Cross(currentDirection, targetDirection);
    //            rb.AddTorque(torqueAxis * turnForce, ForceMode.Force);
    //        }
    //        else
    //        {
    //            // Stop rotating if close to the target direction
    //            rb.angularVelocity = Vector3.zero;
    //        }
    //    }
    //}

    //void MoveTowardsWaypoint()
    //{
    //    rb.AddForce(transform.forward * moveForce, ForceMode.Force);
    //    rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
    //}

    //public void Move(Vector3 target)
    //{
    //    targetPosition = target;
    //    NavMesh.CalculatePath(transform.position, targetPosition, NavMesh.AllAreas, path);
    //}

    //void ondrawgizmos()
    //{
    //    gizmos.color = color.blue;

    //    if (path != null)
    //    {
    //        for (int i = 0; i < path.corners.length; i++)
    //        {
    //            gizmos.drawsphere(path.corners[i], 0.2f);

    //            if (i < path.corners.length - 1)
    //            {
    //                gizmos.drawline(path.corners[i], path.corners[i + 1]);
    //            }
    //        }
    //    }
    //}
}
