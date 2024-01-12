using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightTank : Unit
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
        var enemies = GetEnemiesInProximity();
        if (enemies.Count > 0)
            if (enemies[0] is IDamageable damageable)
                if (turret != null)
                    turret.Attack(damageable);


    }

    public override void TakeDamage(float amount)
    {
        Health -= amount;
    }
}
