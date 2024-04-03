using System.Collections;
using System.Collections.Generic;
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
        //if enemies in proximity and taskstate is idle, start attack task
        List<IDamageable> closeEnemies = GetEnemiesInProximity();
        if (closeEnemies.Count > 0 && CurrentTask.Priority == TaskPriority.Idle)
        {
            ChaseTask chaseTask = new(this, true);

            ///TODO: FINISH THIS CODE, ITS A DEAD END
            //start attack task to attack closest target
            //StartTask(new AttackTask(target: closeEnemies[0], chaseTarget: false));
        }
    }

    public override void TakeDamage(float amount)
    {
        Health -= amount;
    }
}
