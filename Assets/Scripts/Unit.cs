using NaughtyAttributes;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;
using static UnityEngine.GraphicsBuffer;


enum MovementState
{
    Idle,
    Chasing,
    Following,
    Attacking
}

/// <summary>
/// Base class of a unit
/// TODO: Make this more composited
/// </summary>
public class Unit : MonoBehaviour, ISelectable, IDamageable, IAIControllable, ITeamable
{

    private GameObject movementTarget;
    private MovementState currentMoveState;

    //private and not meant to be shown in inspector
    private float timer = 0f;
    private float timerIntervalInSeconds = 2f;

    //Shown in inspector
    [Header("Unit related")]
    [SerializeField] float thresholdDistance = 2.5f;
    //[SerializeField] private float downwardForce = 9.81f; //Controls gravity

    [SerializeField] private UnitTask currentTask = null;
    [SerializeField] private float health = 100f;

    [SerializeField] private float unitSpeed = 5f;
    [SerializeField] private float attackRange = 25f;

    [SerializeField, ReadOnly] private Action movingSuccess;

    [SerializeField] private bool taskDebugInfo = true;

    [SerializeField] protected GameObject[] visualObjects;

    [SerializeField] private Transform selectableHighlightParent;

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
    internal MovementState currentMovementState { get => CurrentMovementState; set => CurrentMovementState = value; }

    private GameObject _instantiatedObject = null;


    public bool IsMoving { get; set; }

    public UnitTask CurrentTask { get => currentTask; private set => currentTask = value; }
    public bool TaskDebugInfo { get => taskDebugInfo; set => taskDebugInfo = value; }
    public float UnitSpeed { get => unitSpeed; set => unitSpeed = value; }
    public NavMeshAgent Agent { get => agent; set => agent = value; }
    //public Action MovingSuccess { get => movingSuccess; set => movingSuccess = value; }
    public float ConstructionMultiplier { get => constructionMultiplier; set => constructionMultiplier = value; }
    public Team OwnedByTeam { get => ownedByTeam; set => ownedByTeam = value; }
    public float Health { get => health; set => health = value; }
    public float AttackRange { get => attackRange; set => attackRange = value; }
    internal MovementState CurrentMovementState { get => currentMoveState; set => currentMoveState = value; }
    public GameObject MovementTarget { get => movementTarget; set => movementTarget = value; }

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
        if (timer >= timerIntervalInSeconds)
        {
            // Call the method
            AIUpdate();

            // Reduce the timer to the excess time beyond the interval
            timer -= timerIntervalInSeconds;
        }
        #endregion


        if (IsMoving == true) UpdateMovement();
    }

    private void LateUpdate()
    {
        if (Health < 0)
        {
            Die();
        }
    }

    public virtual void SetTeam(TeamColour teamByColour)
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
        if (this == null || gameObject == null)
        {
            return null;
        }
        return gameObject;
    }


    public void StartTask(UnitTask task)
    {
        if (CurrentTask != null) CurrentTask.Cancel();
        CurrentTask = task;
        task.Begin();
    }

    public void StopAllTasks()
    {
        currentTask = null;
    }

    private void UpdateMovement()
    {
        if (agent.remainingDistance <= thresholdDistance)
        {
            var dist = Vector3.Distance(agent.transform.position, agent.destination);
            if (dist <= thresholdDistance)
            {
                if (CurrentTask is MoveUnitTask)
                    CurrentTask.Complete();
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

    public List<GameObject> GetUnitsAndBuildingsInProximity()
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
    }



    /// <summary>
    /// Virtual so specific types of units can have their own idle animations, the base unit has no idle or attack
    /// </summary>
    public virtual void PlayIdleAnimation() { }
    public virtual void StopIdleAnimation() { }
    public virtual void MeleeAttack() { }
    public virtual void RangedAttack(Unit unit, IDamageable targetUnit = null) { }
    public virtual void StopAttacking() { }


    [ExecuteInEditMode]
    private void OnDrawGizmos()
    {
        Debug.DrawRay(transform.position, -transform.up * thresholdDistance, Color.red);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }

    public virtual void TakeDamage(float amount)
    {

    }

    public virtual void Die()
    {
        Destroy(gameObject);
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


        //Profiler.BeginSample("Unit close by calculation");
        //Unit closestUnit = enemyUnitsInProximity.OrderBy(u => Vector3.Distance(transform.position, u.transform.position)).FirstOrDefault();
        //Debug.Log(closestUnit.ToString());
        //Profiler.EndSample();
    }

    protected List<IDamageable> GetEnemiesInProximity()
    {
        var teamManager = GameManager.Instance.teamManager;
        List<GameObject> teamableObjectsInProximity = GetUnitsAndBuildingsInProximity();

        List<TeamColour> enemyTeams = teamManager.GetEnemyTeams(OwnedByTeam);
        List<IDamageable> enemyObjects = new List<IDamageable>();

        foreach (GameObject obj in teamableObjectsInProximity)
        {
            var iteamable = obj.GetComponent<ITeamable>();
            if (iteamable == null) continue;
            TeamColour teamable = iteamable.GetTeam();
            if (enemyTeams.Contains(teamable))
            {
                enemyObjects.Add(obj.GetComponent<IDamageable>());
            }
        }
        enemyObjects.Sort((x, y) => Vector3.Distance(transform.position, x.GetGameObject().transform.position).CompareTo(Vector3.Distance(transform.position, y.GetGameObject().transform.position)));

        return enemyObjects;
    }

    public TeamColour GetTeam()
    {
        return OwnedByTeam.teamByColour;
    }

    private void OnDestroy()
    {
        GameManager.Instance.unitManager.Units.Remove(this);
    }

    public void MoveToPoint(Vector3 destination)
    {
        Agent.SetDestination(destination);
    }

    public void MoveInProximityOfPoint(Vector3 destination, float proximityDistance)
    {
        Agent.SetDestination(destination);
    }

    /// <summary>
    /// Handles the movement of the unit and should be called approx,. once a second in the AIUpdate method.
    /// </summary>
    protected void HandleMovement()
    {
        switch (CurrentMovementState)
        {
            case MovementState.Idle:
                Agent.stoppingDistance = 0f;
                break;
            case MovementState.Chasing:
                HandleChase();
                break;

            case MovementState.Following:
                HandleFollow();
                break;
            default:
                Agent.stoppingDistance = 0f;
                break;
        }
    }


    public void ResetTargetAndMoveState()
    {
        MovementTarget = null;
        currentMovementState = MovementState.Idle;
    }

    protected virtual void HandleChase()
    {
        if (MovementTarget == null) Debug.LogWarning("Chase target is null.");
        var distanceToMoveWithin = detectionRadius;
        Agent.stoppingDistance = distanceToMoveWithin; //The attack range divided by 4 is to make sure it stops well within range to possibly fire the cannon, even if the target moves.
        Agent.SetDestination(MovementTarget.transform.position);
    }

    protected virtual void HandleFollow()
    {
        if (MovementTarget == null) Debug.LogWarning("Follow target is null.");
        var distanceToMoveWithin = 2f;
        Agent.stoppingDistance = distanceToMoveWithin;
        Agent.SetDestination(MovementTarget.transform.position);
    }

    private void OnGUI()
    {
        if (GameManager.Instance.unitManager.DebugMode)
        {
            //draw text on the screen rect position of the gameobject
            GUIStyle style = new GUIStyle(GUI.skin.label);
            style.fontSize = Mathf.RoundToInt(16f);
            Vector3 tankpositionOnScreen = Camera.main.WorldToScreenPoint(this.transform.position);
            GUI.Label(new Rect(tankpositionOnScreen.x, Screen.height - tankpositionOnScreen.y, 100, 20), "Health: " + Health, style);
        }
    }
}

