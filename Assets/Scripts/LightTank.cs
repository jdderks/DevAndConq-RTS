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
        foreach (var enemy in enemies)
        {
            Debug.Log(enemy.GetGameObject());
        }
        if (enemies.Count > 0)
            if (enemies[0] is IDamageable damageable)
            {
                turret.Attack(damageable);
            }
    }

    public override void TakeDamage(float amount)
    {
        base.TakeDamage(amount);
    }
}
