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
    AttackUnits = 1,
    Factory = 2,
    Danger = 3
}


public class UtilitySystemEnemyAI : MonoBehaviour, IAIControllable, IAIEnemyBase
{
    //Ai Enemy Timings
    [ReadOnly, SerializeField, ProgressBar(2f, EColor.Blue)] private float timer = 0f;
    private float timerIntervalInSeconds = 2f;

    private int bullDozersInQueue = 0;
    private int amountOfUnitsOnHold = 0;


    //private Team ownedByTeam;
    [ReadOnly, SerializeField] private List<CommandCenter> enemyCommandCenters = new();

    [SerializeField, ReadOnly] private AIBaseState currentBaseState = AIBaseState.None;
    [SerializeField, ReadOnly] private DesirePriority currentDesire = DesirePriority.None;

    [SerializeField] private List<Building> ownedBuildings = new();
    [SerializeField] private List<Unit> ownedUnits = new();

    [SerializeField] private float dangerDetectionRadius = 25;

    [SerializeField] private CommandCenter controllingCommandCenter; //This is the main structure of the AI base, if destroyed the base is lost.
    [SerializeField] private BuildingPositioner buildingPositioner; //This determines where the AI enemy can construct buildings

    [SerializeField, ProgressBar("Constructor Desire", 1)] private float desiredConstructors = 0.5f;
    [SerializeField, ProgressBar("Attacker Desire", 1)] private float desiredOffensiveUnits = 0.5f;
    [SerializeField, ProgressBar("Factory Desire", 1)] private float desiredFactories = 0.5f;
    [SerializeField, ProgressBar("CommandCenterDanger", 1)] private float commandCenterDanger = 0.5f;

    [SerializeField] private AIDesireScriptableObject constructorDesireObject;
    [SerializeField] private AIWarFactoryDesireObject factoryDesireObject;
    [SerializeField] private AIDesireScriptableObject commandCenterDangerObject;
    [SerializeField] private AIUnitDesireScriptableObject offensiveUnitDesireObject;

    private void Start()
    {
        ownedBuildings.Add(controllingCommandCenter);
        amountOfUnitsOnHold = Random.Range(5, 15);
        SetCommandCenters();

        //Set enemy command centers
        //Give command for construction of a bulldozer
    }

    private void SetCommandCenters()
    {
        CommandCenter[] commandCenters = FindObjectsByType<CommandCenter>(FindObjectsSortMode.None);

        foreach (var commandCenter in commandCenters)
        {
            if (controllingCommandCenter.ownedByTeam.enemies.Contains(commandCenter.ownedByTeam.teamByColour))
            {
                enemyCommandCenters.Add(commandCenter);
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
            case DesirePriority.AttackUnits:
                foreach (Building building in ownedBuildings)
                {
                    if (building is WarFactory factory)
                        if (factory.Interactable)
                            factory.actionQueue.AddToActionQueue(factory.GetActions().FirstOrDefault(), ownedUnits);
                    List<LightTank> tanks = new List<LightTank>();

                    foreach (var unit in ownedUnits)
                    {
                        if (unit is LightTank lightTank)
                        {
                            tanks.Add(lightTank);
                        }
                    }
                    if (tanks.Count >= amountOfUnitsOnHold)
                    {
                        foreach (LightTank tank in tanks)
                        {
                            ownedUnits.Remove(tank);
                            if (enemyCommandCenters.Any())
                                tank.StartTask(new MoveUnitTask(tank, enemyCommandCenters[Random.Range(0, enemyCommandCenters.Count)].transform.position));
                            else
                                tank.StartTask(new MoveUnitTask(tank, controllingCommandCenter.transform.position));
                        }
                        amountOfUnitsOnHold = Random.Range(5, 15);
                    }
                }
                break;
            case DesirePriority.Factory:
                List<Unit> bulldozers = new List<Unit>();
                foreach (var unit in ownedUnits)
                {
                    if (unit is ConstructionDozer)
                    {
                        ConstructionDozer constructionDozer = (ConstructionDozer)unit;

                        if (constructionDozer.CurrentTask != null && constructionDozer.CurrentTask.Priority == TaskPriority.Idle)
                        {
                            bulldozers.Add(unit);
                        }
                    }
                }

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
        desiredConstructors = constructorDesireObject.CalculateDesire(dozerAmount + bullDozersInQueue, 1);
        desiredFactories = factoryDesireObject.CalculateDesire(warFactoryAmount, dozerAmount);
        commandCenterDanger = commandCenterDangerObject.CalculateDesire(GetEnemiesInProximity().Count);
        desiredOffensiveUnits = offensiveUnitDesireObject.CalculateDesire(warFactoryAmount, ownedUnits.Count);

        // Determine the highest desire and return the corresponding DesirePriority
        float maxDesire = Mathf.Max(desiredConstructors, desiredOffensiveUnits, desiredFactories, commandCenterDanger);

        if (maxDesire < 0.5f) //no tasks have a real priority right now so just do nothing for a bit
        {
            return DesirePriority.None; // Default case
        }

        if (maxDesire == desiredConstructors)
            return DesirePriority.Constructor;
        else if (maxDesire == desiredFactories)
            return DesirePriority.Factory;
        else if (maxDesire == desiredOffensiveUnits)
            return DesirePriority.AttackUnits;
        else if (maxDesire == commandCenterDanger)
            return DesirePriority.Danger;

        return DesirePriority.None; // Default case
    }


    [Button("Construct Dozer")]
    public void AddConstructionDozerToQueue()
    {
        RtsAction dozerAction = controllingCommandCenter.GetActions().FirstOrDefault();
        RtsQueueAction queueAction = controllingCommandCenter.actionQueue.AddToActionQueue(dozerAction, ownedUnits);
        bullDozersInQueue += 1;
        queueAction.OnActivate += () =>
        {
            bullDozersInQueue -= 1;
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