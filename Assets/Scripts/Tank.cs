using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : Unit
{
    private GameObject turret;

    private float idleTurretRotationRange = 30f;


    public override void PlayIdleAnimation()
    {
        base.PlayIdleAnimation();
    }
    public override void RangedAttack(Vector3 position ,Unit targetUnit = null)
    {
        base.RangedAttack(position);
    }
}
