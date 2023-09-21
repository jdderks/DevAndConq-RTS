using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : Unit
{
    private GameObject turret;
    private float idleTurretRotationRange = 30f;
    private float turretRotationSpeed = 5f; // Adjust this value to control rotation speed
    private Quaternion originRotation = Quaternion.Euler(0,0,0);
    private Quaternion initialRotation;
    private Quaternion targetRotation;
    private Coroutine idleCoroutine = null;
    private Coroutine resetRotationCoroutine = null;

    void Start()
    {
        GameManager.Instance.unitManager.RegisterUnit(this);
        turret = transform.Find("Turret").gameObject; // Assuming the turret is a child object named "Turret"
        //originRotation = turret.transform.rotation;
        initialRotation = turret.transform.rotation;
    }

    public override void PlayIdleAnimation()
    {
        idleCoroutine = StartCoroutine(IdleTurretRotation());
    }

    public override void RangedAttack(Vector3 position, Unit targetUnit = null)
    {
        base.RangedAttack(position);
    }

    public override void RangedAttack(Unit unit, Unit targetUnit = null)
    {
        base.RangedAttack(unit.transform.position);
    }

    public override void StopIdleAnimation()
    {
        StopCoroutine(idleCoroutine);
        idleCoroutine = StartCoroutine(IdleTurretRotation(true));
        turret.transform.rotation = initialRotation;
    }

    public IEnumerator IdleTurretRotation(bool resetRotation = false)
    {
        while (true)
        {
            if (!resetRotation)
            {
                targetRotation = Quaternion.Euler(
                    turret.transform.localEulerAngles.x,
                    turret.transform.localEulerAngles.y + Random.Range(-idleTurretRotationRange, idleTurretRotationRange),
                    turret.transform.localEulerAngles.z
                );
            }
            else
            {
                targetRotation = originRotation;
            }

            float elapsedTime = 0f;
            while (elapsedTime < 1f)
            {
                turret.transform.localRotation = Quaternion.Slerp(initialRotation, targetRotation, elapsedTime);
                elapsedTime += Time.deltaTime * turretRotationSpeed;
                yield return null;
            }
            if (!resetRotation)
            {
                yield return new WaitForSeconds(Random.Range(1, 5));
                initialRotation = turret.transform.localRotation;
                targetRotation = default;
            }
            else
            {
                yield break;
            }
        }
    }

}