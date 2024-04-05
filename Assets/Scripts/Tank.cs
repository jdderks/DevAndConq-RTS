using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Android;



public class Tank : Unit
{
    //private Coroutine resetRotationCoroutine = null;
    [SerializeField] private Turret turret;
    [SerializeField] private bool turretActiveWhileDriving = true;


    //[SerializeField] private GameObject turret;

    void Start()
    {
        GameManager.Instance.unitManager.RegisterUnit(this);
    }

    public override void AIUpdate()
    {
        HandleMovement();
    }

    public override void TakeDamage(float amount)
    {
        Health -= amount;
    }

    protected override void HandleChase()
    {
        if (MovementTarget == null) Debug.LogWarning("Chase target is null.");
        if (Vector3.Distance(this.transform.position, MovementTarget.transform.position) < AttackRange)
        {
            if (MovementTarget.GetComponent<IDamageable>() is IDamageable damageable)
                turret.Attack(damageable);
        }
        var distanceToMoveWithin = AttackRange - (AttackRange / 4);
        Agent.stoppingDistance = distanceToMoveWithin; //The attack range divided by 4 is to make sure it stops well within range to possibly fire the cannon, even if the target moves.
        Agent.SetDestination(MovementTarget.transform.position);


    }
}
