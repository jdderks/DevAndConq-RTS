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
    private float timerInterval = 3f;

    private Quaternion originRotation = Quaternion.Euler(-90, 0, 0);
    private Quaternion initialRotation;
    private Quaternion targetRotation;

    [SerializeField] private float idleTurretRotationRange = 30f;
    [SerializeField] private float turretRotationSpeed = 5f;
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
                targetRotation = originRotation;
                break;
            case TurretState.Idle:
                IdleBehaviour();
                break;
            case TurretState.Attacking:
                FireTurret();
                break;
            default:
                break;
        }
    }

    private void FireTurret()
    {
        if (target)
        {
            return;
        }
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
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget.normalized);

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
        // Update the time since the last fire
        timeSinceLastFire += Time.deltaTime;

        // Check if the turret can fire based on the fire rate
        if (timeSinceLastFire >= 1f / fireRatePerSeconds)
        {
            // Implement firing logic here
            // This method will be called when the rotation to the target is within the threshold
            // You can handle firing rounds or other actions related to attacking the target

            // Call the Fire method
            Fire();

            // Reset the time since the last fire
            timeSinceLastFire = 0f;
        }
    }

    private void Fire()
    {
        Debug.Log("Boom, fired at" + target);
        if (target != null)
            target.TakeDamage(damage);
        else
        {
            target = null;
            StopAttacking();
        }
    }


    private void IdleBehaviour()
    {
        idleTimer += Time.deltaTime;
        if (idleTimer >= timerInterval)
        {
            // Call the method
            targetRotation = Quaternion.Euler(
            transform.localEulerAngles.x,
            transform.localEulerAngles.y + Random.Range(-idleTurretRotationRange, idleTurretRotationRange),
            transform.localEulerAngles.z
        );

            idleTimer -= timerInterval;
        }
    }
}

