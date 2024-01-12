using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public enum TurretState
{
    None = -1,
    Idle = 0,
    Attacking = 1
}

public class Turret : MonoBehaviour
{
    private float idleTimer = 0f;
    private float timerInterval = 5f;

    private Quaternion originRotation = Quaternion.Euler(-90, 0, 0);
    private Quaternion initialRotation;
    private Quaternion targetRotation;

    [SerializeField] private GameObject parent;

    [SerializeField] private float idleTurretRotationRange = 30f;
    [SerializeField] private float turretRotationSpeed = 50f;
    [SerializeField] private float fireRange = 20f;


    [SerializeField] private float damage = 12.5f;
    [SerializeField] private float fireRatePerSeconds = 2f;
    private float timeSinceLastFire = 0f;

    private TurretState turretState = TurretState.Idle;

    private IDamageable target;

    public TurretState TurretState { get => turretState; set => turretState = value; }

    private void Start()
    {
        initialRotation = transform.rotation;
    }

    public void Attack(IDamageable target)
    {
        this.target = target;
        turretState = TurretState.Attacking;
    }

    public void StopAttacking()
    {
        this.target = null;
        turretState = TurretState.Idle;
    }

    void Update()
    {
        switch (TurretState)
        {
            case TurretState.None:
                //ReturnToIdle();
                break;
            case TurretState.Idle:
                IdleBehaviour();
                break;
            case TurretState.Attacking:
                if (this == null)
                    return;
                if (target != null || target.GetGameObject() != null)
                    RotateTurretTowardsEnemy();
                else
                    StopAttacking();
                break;
            default:
                break;
        }
    }


    private void RotateTurretTowardsEnemy()
    {
        var targetTransform = target.GetGameObject().transform;

        // Check if the target is within fire range
        if (Vector3.Distance(transform.position, targetTransform.position) > fireRange)
        {
            StopAttacking();
        }
        else
        {
            // Set the turret state to idle

            // Calculate the rotation to point at the target
            Vector3 directionToTarget = targetTransform.position - transform.position;
            targetRotation = Quaternion.LookRotation(directionToTarget.normalized);

            // Set the targetRotation
            targetRotation *= Quaternion.Euler(-90, 0, 0); // Replace with the appropriate rotation adjustment

            // Use RotateTowards to ensure the rotation adheres to the maximum turret rotation speed
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turretRotationSpeed * Time.deltaTime);

            // Check if the rotation to the target is less than 2 degrees
            if (Quaternion.Angle(transform.rotation, targetRotation) < 2f)
            {
                // Keep firing rounds at a firerate interval
                FireRounds();
            }
        }
    }


    private void FireRounds()
    {
        timeSinceLastFire += Time.deltaTime;

        if (timeSinceLastFire >= 1f / fireRatePerSeconds)
        {
            Fire();
            timeSinceLastFire = 0f;
        }
    }

    private void Fire()
    {
        Debug.Log("Boom, fired at" + target);
        target.TakeDamage(damage);
        StopAttacking();
    }

    private void IdleBehaviour()
    {
        idleTimer += Time.deltaTime;

        if (idleTimer >= timerInterval)
        {
            Quaternion baseRotation = Quaternion.LookRotation(parent.transform.forward, Vector3.up) * Quaternion.Euler(-90, 0, 0);

            // Apply a random rotation within the specified range only on the Y-axis
            float randomRotation = Random.Range(-idleTurretRotationRange, idleTurretRotationRange);
            targetRotation = Quaternion.Euler(baseRotation.eulerAngles.x, baseRotation.eulerAngles.y + randomRotation, baseRotation.eulerAngles.z);

            idleTimer -= timerInterval;
        }

        // Use RotateTowards to ensure the rotation adheres to the maximum turret rotation speed
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turretRotationSpeed * Time.deltaTime);
    }





    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue; // Set the color of the arrow

        // Draw an arrow indicating the targetRotation
        Gizmos.DrawRay(transform.position, parent.transform.forward * 10);
    }

}

