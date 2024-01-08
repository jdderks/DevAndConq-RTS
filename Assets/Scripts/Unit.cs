using NaughtyAttributes;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public enum UnitType
{
    Humanoid = 0,    //Infantry unit
    Quadricycle = 1, //For all car-alikes
    TwoWheeler = 2,  //For bicycles and motorcycles.
    AirPlane = 3,    //For jet aircraft which fly with wings.
    VTOL = 4         //For aircraft that can hover and land and take off vertically
}

public class Unit : MonoBehaviour, ISelectable, IDamageable
{
    [Header("Unit related")]
    [SerializeField] float thresholdDistance = 2.5f;
    [SerializeField] private float downwardForce = 9.81f; //Controls gravity

    [SerializeField] private float unitSpeed = 5f;

    [SerializeField, ReadOnly] private Action movingSuccess;

    [SerializeField] private bool taskDebugInfo = true;

    [SerializeField] private Transform selectableHighlightParent;
    [SerializeField] private UnitType unitType;

    [SerializeField] private NavMeshAgent agent;

    [SerializeField] private Team ownedByTeam;

    [SerializeField] private float constructionMultiplier = 1;
    [SerializeField] private float detectionRadius = 20;

    private List<RtsAction> unitRtsActions = new();

    private List<RtsUnitAction> _rtsActions = new(8); //Emtpy RTS unit slots, maximum of 8

    public List<RtsUnitAction> RtsActions
    {
        get => _rtsActions;
        set => _rtsActions = value;
    }

    private GameObject _instantiatedObject = null;


    public bool IsMoving { get; set; }

    public UnitType UnitType { get => unitType; set => unitType = value; }
    public UnitTask CurrentTask { get; private set; }
    public bool TaskDebugInfo { get => taskDebugInfo; set => taskDebugInfo = value; }
    public float UnitSpeed { get => unitSpeed; set => unitSpeed = value; }
    public NavMeshAgent Agent { get => agent; set => agent = value; }
    public Action MovingSuccess { get => movingSuccess; set => movingSuccess = value; }
    public float ConstructionMultiplier { get => constructionMultiplier; set => constructionMultiplier = value; }

    private void Start()
    {
        //agent = GetComponent<NavMeshAgent>();
        GameManager.Instance.unitManager.RegisterUnit(this);
        foreach (Transform t in transform)
        {
            if (t.name == "Selection")
            {
                selectableHighlightParent = t;
            }
        }
    }

    public void SetTeam(TeamScriptableObject team)
    {
        ownedByTeam = team;
    }

    public void Select()
    {
        Assert.IsNotNull(selectableHighlightParent, "Parent object not set in prefab.");
        _instantiatedObject = Instantiate(GameManager.Instance.selectionManager.SelectionPrefab, selectableHighlightParent);
    }

    public void Deselect()
    {
        Destroy(_instantiatedObject);
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }

    public void StartTask(UnitTask task)
    {
        if (CurrentTask != null) CurrentTask.Cancel();
        CurrentTask = task;
        task.Begin();
    }
    private void UpdateMovement()
    {
        if (agent.remainingDistance <= thresholdDistance)
        {
            var dist = Vector3.Distance(agent.transform.position, agent.destination);
            if (dist <= thresholdDistance)
            {
                if (CurrentTask is MoveUnitTask)
                {
                    CurrentTask.Complete();
                }
                else if (CurrentTask is SequenceTask)
                {
                    SequenceTask seqTask = CurrentTask as SequenceTask;
                    MoveUnitTask moveTask = seqTask.GetCurrentTask() as MoveUnitTask;
                    if (moveTask != null)
                        moveTask.Complete();
                }
            }
        }
    }


    public List<Unit> GetUnitsInDetectionRadius()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius);

        foreach (Collider collider in colliders)
        {
            if (collider.GetComponent<Unit>() is Unit unit)
            {
                if (GameManager.Instance.teamManager.GetEnemyTeams(ownedByTeam).Any())
                {

                }
            }
        }




        return null;
    }


    private void Update()
    {
        //FallToGround();
        if (IsMoving == true) UpdateMovement();
    }

    /// <summary>
    /// Virtual so specific types of units can have their own idle animations, the base unit has no idle or attack
    /// </summary>
    public virtual void PlayIdleAnimation() { }
    public virtual void StopIdleAnimation() { }
    public virtual void MeleeAttack() { }
    public virtual void RangedAttack(Vector3 position, Unit targetUnit = null) { }
    public virtual void RangedAttack(Unit unit, Unit targetUnit = null) { }
    public virtual void Hit() { }

    [ExecuteInEditMode]
    private void OnDrawGizmos()
    {
        Debug.DrawRay(transform.position, -transform.up * thresholdDistance, Color.red);
    }

    public virtual float TakeDamage()
    {
        return 0f;
    }

    public virtual void Die()
    {
        return;
    }

    public virtual List<RtsAction> GetActions()
    {
        return unitRtsActions;
    }

    public virtual ActionQueue GetActionQueue()
    {
        Debug.LogError("Base units don't have an action queue by default! This void can be overridden.");
        return null;
    }
}
