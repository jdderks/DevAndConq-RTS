using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using System.Linq;
using UnityEditor.UI;

public enum AIBaseState
{
    None = -1,
    Settling = 0,
    Base = 1,
    EndGame = 2
}

public enum DesirePriority
{
    None = -1,
    Constructor = 0,
    Attacker = 1,
    Defender = 2,
    Factory = 3,
    Danger = 4
}


public class UtilitySystemEnemyAI : MonoBehaviour, IAIControllable, IAIEnemyBase
{
    //Ai Enemy Timings
    private float timer = 0f;
    private float timerIntervalInSeconds = 2f;

    //private Team ownedByTeam;
    private List<CommandCenter> enemyCommandCenters = new();

    [SerializeField, ReadOnly] private AIBaseState currentBaseState = AIBaseState.None;
    [SerializeField, ReadOnly] private DesirePriority currentDesire = DesirePriority.None;

    [SerializeField] private List<Building> ownedBuildings = new();
    [SerializeField] private List<Unit> ownedUnits = new();


    [SerializeField] private float dangerDetectionRadius = 25;

    [SerializeField] private CommandCenter controllingCommandCenter; //This is the main structure of the AI base, if destroyed the base is lost.
    [SerializeField] private BuildingPositioner buildingPositioner; //This determines where the AI enemy can construct buildings

    [SerializeField, ProgressBar("Constructor Desire", 1)] private float desiredConstructors = 0.5f;
    [SerializeField, ProgressBar("Attacker Desire", 1)] private float desiredAttackers = 0.5f;
    [SerializeField, ProgressBar("Defender Desire", 1)] private float desiredDefenders = 0.5f;
    [SerializeField, ProgressBar("Factory Desire", 1)] private float desiredFactories = 0.5f;
    [SerializeField, ProgressBar("CommandCenterDanger", 1)] private float commandCenterDanger = 0.5f;

    [SerializeField] private AIDesireScriptableObject constructorDesireObject;
    [SerializeField] private AIWarFactoryDesireObject factoryDesireObject;
    [SerializeField] private AIDesireScriptableObject commandCenterDangerObject;

    private void Start()
    {
        ownedBuildings.Add(controllingCommandCenter);

        //Set enemy command centers
        //Give command for construction of a bulldozer
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
    }

    public void AIUpdate()
    {
        currentDesire = CalculateDesiresAndDangers();

        switch (currentDesire)
        {
            case DesirePriority.None:
                break;
            case DesirePriority.Constructor:
                AddConstructionDozerToQueue();
                break;
            case DesirePriority.Attacker:
                break;
            case DesirePriority.Defender:
                break;
            case DesirePriority.Factory:
                IEnumerable<Unit> bulldozers = ownedUnits.Where(unit => unit is ConstructionDozer && ((ConstructionDozer)unit).CurrentTask.Priority == TaskPriority.Idle);

                if (bulldozers.Any() &&
                    buildingPositioner.buildingPositions.Any(t => !t.occupied))
                {
                    ConstructionDozer randomBulldozer = bulldozers.ElementAt(UnityEngine.Random.Range(0, bulldozers.Count())) as ConstructionDozer;
                    BuildingPosition buildingPosition = buildingPositioner.GetRandomBuildingPosition(false);
                    var warFactoryPrefab = randomBulldozer.ConstructWarFactoryAction.GetPanelInfo().actionPrefab;
                    var warFactory = GameManager.Instance.buildingManager.InstantiateBuildingAndGiveTask(warFactoryPrefab, buildingPosition.position, randomBulldozer);
                    ownedBuildings.Add(warFactory.GetComponent<WarFactory>());
                    buildingPositioner.SetOccupied(buildingPosition);
                }
                break;
            case DesirePriority.Danger:
                break;
            default:
                break;
        }
    }

    private DesirePriority CalculateDesiresAndDangers()
    {
        // Calculate all desired amounts
        int dozerAmount = ownedUnits.Count(u => u is ConstructionDozer);
        int warFactoryAmount = ownedBuildings.Count(u => u is WarFactory);

        // Calculate desires using the desire objects
        desiredConstructors = constructorDesireObject.CalculateDesire(dozerAmount);
        desiredFactories = factoryDesireObject.CalculateDesire(warFactoryAmount, dozerAmount);
        commandCenterDanger = commandCenterDangerObject.CalculateDesire(GetEnemiesInProximity().Count);

        // Determine the highest desire and return the corresponding DesirePriority
        float maxDesire = Mathf.Max(desiredConstructors, desiredFactories, commandCenterDanger);

        if (maxDesire < 0.5f) //no tasks have a real priority right now so just do nothing for a bit
        {
            return DesirePriority.None; // Default case
        }

        if (maxDesire == desiredConstructors)
            return DesirePriority.Constructor;
        else if (maxDesire == desiredFactories)
            return DesirePriority.Factory;
        else if (maxDesire == commandCenterDanger)
            return DesirePriority.Danger;

        return DesirePriority.None; // Default case
    }


    [Button("Construct Dozer")]
    public void AddConstructionDozerToQueue()
    {
        RtsAction dozerAction = controllingCommandCenter.GetActions().FirstOrDefault();
        RtsQueueAction queueAction = controllingCommandCenter.actionQueue.AddToActionQueue(dozerAction);
        queueAction.OnActivate += () =>
        {
            GameObject activatedObject = queueAction.Action.Activate();
            if (activatedObject.GetComponent<Unit>() is Unit unit)
            {
                ownedUnits.Add(unit);
            }
        };
    }


    protected List<ITeamable> GetEnemiesInProximity()
    {
        var teamManager = GameManager.Instance.teamManager;
        List<ITeamable> enemyObjects = new List<ITeamable>();

        Collider[] colliders = Physics.OverlapSphere(controllingCommandCenter.transform.position, dangerDetectionRadius);

        List<string> enemyTags = new List<string>();
        foreach (var enemyTeam in teamManager.GetEnemyTeams(controllingCommandCenter.ownedByTeam))
        {
            Team team = teamManager.GetTeamByColour(enemyTeam);
            enemyTags.Add(team.teamTagName);
        }

        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.layer == LayerMask.NameToLayer("Building") ||
                collider.gameObject.layer == LayerMask.NameToLayer("Unit"))
            {
                if (collider.gameObject == gameObject) continue;

                ITeamable teamableObject = collider.gameObject.GetComponent<ITeamable>();

                if (teamableObject != null && enemyTags.Contains(collider.gameObject.tag))
                {
                    enemyObjects.Add(teamableObject);
                }
            }
        }

        return enemyObjects;
    }

    private void OnDrawGizmos()
    {

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(controllingCommandCenter.transform.position, dangerDetectionRadius);
    }


}
