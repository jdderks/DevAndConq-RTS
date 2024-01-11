using NaughtyAttributes;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Profiling;
using UnityEngine.UIElements;

public enum UnitType
{
    Humanoid = 0,    //Infantry unit
    Quadricycle = 1, //For all car-alikes
    TwoWheeler = 2,  //For bicycles and motorcycles.
    AirPlane = 3,    //For jet aircraft which fly with wings.
    VTOL = 4         //For aircraft that can hover and land and take off vertically
}

public class Unit : MonoBehaviour, ISelectable, IDamageable, IAIControllable, ITeamable
{
    //private and not meant to be shown in inspector
    private float timer = 0f;
    private float timerInterval = 1f;


    //Shown in inspector
    [Header("Unit related")]
    [SerializeField] float thresholdDistance = 2.5f;
    //[SerializeField] private float downwardForce = 9.81f; //Controls gravity

    [SerializeField] private float unitSpeed = 5f;

    [SerializeField, ReadOnly] private Action movingSuccess;

    [SerializeField] private bool taskDebugInfo = true;

    [SerializeField] protected GameObject[] visualObjects;

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
    public Team OwnedByTeam { get => ownedByTeam; set => ownedByTeam = value; }

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
    private void Update()
    {
        #region AI Update Timing
        // Increment the timer by the deltaTime
        timer += Time.deltaTime;

        // Check if the timer exceeds or equals the interval
        if (timer >= timerInterval)
        {
            // Call the method
            AIUpdate();

            // Reduce the timer to the excess time beyond the interval
            timer -= timerInterval;
        }
        #endregion


        if (IsMoving == true) UpdateMovement();
    }

    public virtual void SetTeam(TeamByColour teamByColour)
    {
        OwnedByTeam = GameManager.Instance.teamManager.GetTeamByColour(teamByColour);

        for (int i = 0; i < visualObjects.Length; i++)
        {
            GameObject item = visualObjects[i];
            Renderer renderer = item.GetComponent<Renderer>();
            var mats = renderer.materials;
            for (int j = 0; j < mats.Length; j++)
            {
                if (mats[j].name.Contains("Team_color"))
                    mats[j] = OwnedByTeam.teamMaterial;
            }

            //mats[1] = OwnedByTeam.teamMaterial;
            renderer.materials = mats;
        }

        tag = OwnedByTeam.teamTagName;
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

    public List<GameObject> GetTeamableObjectsInProximity()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius);
        var gameObjects = new List<GameObject>();
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.layer == LayerMask.NameToLayer("Building") ||
                collider.gameObject.layer == LayerMask.NameToLayer("Unit"))
            {
                if (collider.gameObject == gameObject) continue;

                gameObjects.Add(collider.gameObject);
            }
        }
        return gameObjects;
        //Debug.Log("Amount of units nearby: " + unitList.Count);
        //return unitList;
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

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
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

    //This is only ran a couple times a second for optimisation
    public virtual void AIUpdate()
    {
        var enemies = GetEnemiesInProximity();
        foreach (var enemy in enemies)
        {
            Debug.Log(enemy.GetGameObject());
        }

        //Profiler.BeginSample("Unit close by calculation");
        //Unit closestUnit = enemyUnitsInProximity.OrderBy(u => Vector3.Distance(transform.position, u.transform.position)).FirstOrDefault();
        //Debug.Log(closestUnit.ToString());
        //Profiler.EndSample();
    }

    private List<ITeamable> GetEnemiesInProximity()
    {
        var teamManager = GameManager.Instance.teamManager;
        List<GameObject> teamableObjectsInProximity = GetTeamableObjectsInProximity();

        List<TeamByColour> enemyTeams = teamManager.GetEnemyTeams(OwnedByTeam);

        var enemyTags = new List<string>();
        foreach (var enemyTeam in enemyTeams)
        {
            Team team = teamManager.GetTeamByColour(enemyTeam);
            enemyTags.Add(team.teamTagName);
        }

        List<ITeamable> enemyObjects = new List<ITeamable>();
        foreach (var obj in teamableObjectsInProximity)
        {
            if (enemyTags.Contains(obj.tag))
            {
                enemyObjects.Add(obj.GetComponent<ITeamable>());
            }
        }

        return enemyObjects;
    }

    public void SetAIController()
    {
        throw new NotImplementedException();
    }



}
