using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Android;



public class Tank : Unit
{
    //private Coroutine resetRotationCoroutine = null;
    [SerializeField] private Turret turret;
    [SerializeField] private bool turretActiveWhileDriving = true;

    private bool attackAnyEnemyWithinRange = true;

    //[SerializeField] private GameObject turret;

    void Start()
    {
        GameManager.Instance.unitManager.RegisterUnit(this);
    }

    public override void AIUpdate()
    {
        HandleMovement();
        if (attackAnyEnemyWithinRange && turret.TurretState == TurretState.Idle) AttackUnitsWithinRange();
    }

    private void AttackUnitsWithinRange()
    {
        List<IDamageable> enemiesInProximity = GetEnemiesInProximity();
        if (enemiesInProximity.Count > 0)
            if (enemiesInProximity[0] is IDamageable damageable)
                if (turret != null)
                    turret.Attack(damageable);
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


    //public void AIUpdate()
    //{
    //    var enemies = GetEnemiesInProximity();
    //    if (enemies.Count > 0)
    //        if (enemies[0] is IDamageable damageable)
    //            if (turret != null)
    //                turret.Attack(damageable);
    //}

    //protected List<ITeamable> GetEnemiesInProximity()
    //{
    //    var teamManager = GameManager.Instance.teamManager;
    //    List<GameObject> teamableObjectsInProximity = GetUnitsAndBuildingsInProximity();

    //    List<TeamColour> enemyTeams = teamManager.GetEnemyTeams(ownedByTeam);

    //    List<string> enemyTags = new List<string>();
    //    foreach (var enemyTeam in enemyTeams)
    //    {
    //        Team team = teamManager.GetTeamByColour(enemyTeam);
    //        enemyTags.Add(team.teamTagName);
    //    }

    //    List<ITeamable> enemyObjects = new List<ITeamable>();
    //    foreach (var obj in teamableObjectsInProximity)
    //    {
    //        if (enemyTags.Contains(obj.tag))
    //        {
    //            enemyObjects.Add(obj.GetComponent<ITeamable>());
    //        }
    //    }

    //    return enemyObjects;
    //}
}
