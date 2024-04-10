using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using System.Linq;
using Unity.VisualScripting;
using NUnit.Framework;

public enum DesirePriority
{
    None = -1,
    Constructor = 0,
    AttackUnits = 1,
    Factory = 2,
    Danger = 3,
    Defend = 4,
    SupplyCenter = 5,
    SupplyTruck = 6
}

public class UtilitySystemEnemyAI : MonoBehaviour, IAIControllable, IAIEnemyBase
{
    //Ai Enemy Timings
    [ReadOnly, SerializeField, ProgressBar(1f, EColor.Blue)] private float timer = 0f;
    private float timerIntervalInSeconds = 1f;

    private int bullDozersInQueue = 0;
    private int amountOfUnitsOnHold = 0;

    //private Team ownedByTeam;
    [ReadOnly, SerializeField] private List<CommandCenter> enemyCommandCenters = new();

    //[SerializeField, ReadOnly] private AIBaseState currentBaseState = AIBaseState.None;
    [SerializeField, ReadOnly] private DesirePriority currentDesire = DesirePriority.None;

    [SerializeField] private List<Building> ownedBuildings = new();
    [SerializeField] private List<Unit> ownedUnits = new();

    [SerializeField] private float dangerDetectionRadius = 25;

    [SerializeField] private CommandCenter controllingCommandCenter; //This is the main structure of the AI base, if destroyed the base is lost.
    [SerializeField] private BuildingPositioner buildingPositioner; //This determines where the AI enemy can construct buildings
    [SerializeField] private SupplyDock closestSupplyDock; //The closest supplyDock for economy sake.


    [SerializeField, ProgressBar("Constructor Desire", 1)] private float desiredConstructors = 0.5f;
    [SerializeField, ProgressBar("Attacker Desire", 1)] private float desiredOffensiveUnits = 0.5f;
    [SerializeField, ProgressBar("Defender Desire", 1)] private float desiredDefenses = 0.5f;
    [SerializeField, ProgressBar("Factory Desire", 1)] private float desiredFactories = 0.5f;
    [SerializeField, ProgressBar("SupplyCenter Desire", 1)] private float desiredSupplyCenters = 0.5f;
    [SerializeField, ProgressBar("SupplyTruck Desire", 1)] private float desiredSupplyTrucks = 0.5f;
    [SerializeField, ProgressBar("CommandCenter Danger", 1, EColor.Orange)] private float commandCenterDanger = 0.5f;

    [SerializeField] private AIDesireScriptableObject constructorDesireObject;
    [SerializeField] private AIWarFactoryDesireObject factoryDesireObject;
    [SerializeField] private AIDesireScriptableObject commandCenterDangerObject;
    [SerializeField] private AIUnitDesireScriptableObject offensiveUnitDesireObject;
    [SerializeField] private AIDefensesDesireScriptableObject defensiveUnitDesireObject;
    [SerializeField] private AISupplyResourceDesireScriptableObject supplyResourceDesireObject;

    [Header("Personality related")]
    //[SerializeField] private AIPersonalityScriptableObject currentPersonality;
    private float loadedAggresiveness = 0;
    private float loadedDefensiveness = 0;

    private void Start()
    {
        ownedBuildings.Add(controllingCommandCenter);
        SetCommandCenters();
        //if (currentPersonality == null)
        //{
        //    currentPersonality = GameManager.Instance.personalityManager.GetRandomPersonality();
        //}
        amountOfUnitsOnHold = GetArmySize();
        LoadPersonalitiesFromPrefs();
    }

    public void LoadPersonalitiesFromPrefs()
    {
        loadedAggresiveness = PlayerPrefs.GetFloat("Aggressiveness");
        loadedDefensiveness = PlayerPrefs.GetFloat("Defensiveness");

        if (loadedAggresiveness == 0 || loadedDefensiveness == 0)
        {
            Debug.LogError("Personality not loaded correctly!");
        }
        
        //Debug.Log("Loaded personalities!");
    }

    private int GetArmySize()
    {
        int size = UnityEngine.Random.Range(5, 15);
        size *= (int)loadedAggresiveness;
        return size;
    }

    private void SetCommandCenters()
    {
        var commandCenters = GameManager.Instance.teamManager.GetAllCommandCenters();

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
        SetCurrentDesire();
    }

    private void SetCurrentDesire()
    {
        Debug.Log("Current AI desire is: " + currentDesire.ToString());
        switch (currentDesire)
        {
            case DesirePriority.None:
                //Debug.Log("Why is the prioritystate none?");
                break;
            case DesirePriority.Constructor:
                AddConstructionDozerToQueue();
                break;
            case DesirePriority.AttackUnits:
                PrepareAttack();
                break;
            case DesirePriority.Factory:
                ConstructFactory();
                break;
            case DesirePriority.Danger:
                break;
            case DesirePriority.Defend:
                ConstructDefenses();
                break;
            case DesirePriority.SupplyCenter:
                ConstructSupplyCenter();
                break;
            //case DesirePriority.SupplyTruck:
            //    //This should check for whether or not there is a supply center but no truck,
            //    //if that's the case the existing supply center should spawn a truck
            //    break;
            default:
                break;
        }
    }

    private void ConstructFactory()
    {
        List<Unit> bulldozers = GetAvailableBulldozers();

        if (bulldozers.Any() &&
            buildingPositioner.buildingPositions.Any(t => !t.occupied))
        {
            ConstructionDozer randomBulldozer = bulldozers.ElementAt(UnityEngine.Random.Range(0, bulldozers.Count())) as ConstructionDozer;
            BuildingPosition buildingPosition = buildingPositioner.GetRandomBuildingPosition(false);
            GameObject warFactoryPrefab = randomBulldozer.ConstructWarFactoryAction.GetPanelInfo().actionPrefab;
            GameObject warFactory = GameManager.Instance.buildingManager.InstantiateBuildingAndGiveTask(warFactoryPrefab, buildingPosition.position, randomBulldozer);
            ownedBuildings.Add(warFactory.GetComponent<WarFactory>());
            buildingPositioner.SetOccupied(buildingPosition);
        }
    }

    private void ConstructSupplyCenter()
    {
        List<Unit> bulldozers = GetAvailableBulldozers();

        if (bulldozers.Any() && closestSupplyDock != null)
        {
            BuildingPosition supplyCenterPosition = buildingPositioner.GetSupplyDockPosition();
            ConstructionDozer bullDozerUnit = ArrayHelpers.GetRandomElement(bulldozers) as ConstructionDozer;
            if (supplyCenterPosition.occupied) return;

            GameObject supplyCenterPrefab = bullDozerUnit.ConstructSupplyCenter.GetPanelInfo().actionPrefab;
            GameObject supplyCenter = GameManager.Instance.buildingManager.InstantiateBuildingAndGiveTask(supplyCenterPrefab, supplyCenterPosition.position, bullDozerUnit);
            ownedBuildings.Add(supplyCenter.GetComponent<SupplyCenter>());

            buildingPositioner.SetOccupied(supplyCenterPosition);
        }
    }

    private List<Unit> GetAvailableBulldozers()
    {
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
        return bulldozers;
    }

    private void ConstructDefenses()
    {
        List<Unit> bulldozers = GetAvailableBulldozers();

        if (bulldozers.Any() && controllingCommandCenter.DefensivePositioner.buildingPositions.Any(t => !t.occupied))
        {
            ConstructionDozer randomBulldozer = bulldozers.ElementAt(UnityEngine.Random.Range(0, bulldozers.Count())) as ConstructionDozer;
            BuildingPosition buildingPosition;
            if (enemyCommandCenters.Any())
                buildingPosition = controllingCommandCenter.DefensivePositioner.GetBuildingPositionClosestToTransform(enemyCommandCenters[0].transform, false);
            else
                buildingPosition = controllingCommandCenter.DefensivePositioner.GetRandomBuildingPosition(false);
            var defensesPrefab = randomBulldozer.ConstructTurretAction.GetPanelInfo().actionPrefab;
            var warFactory = GameManager.Instance.buildingManager.InstantiateBuildingAndGiveTask(defensesPrefab, buildingPosition.position, randomBulldozer);
            ownedBuildings.Add(warFactory.GetComponent<WarFactory>());
            buildingPositioner.SetOccupied(buildingPosition);
        }
    }

    private void PrepareAttack()
    {
        foreach (Building building in ownedBuildings)
        {
            if (building is not WarFactory) continue;

            List<Tank> tanks = new List<Tank>();
            var enemyturrets = GetTurrets(enemyCommandCenters[0].ownedByTeam);

            if (building is WarFactory factory)
            {
                if (!factory.Interactable) continue;

                RtsAction actionToPrepare = GetProduceTankAction(tanks, enemyturrets, factory);

                if (actionToPrepare != null)
                    factory.actionQueue.AddToActionQueue(actionToPrepare, ownedUnits);

            }

            foreach (var unit in ownedUnits)
            {
                if (unit is Tank lightTank)
                {
                    tanks.Add(lightTank);
                }
            }
            if (tanks.Count >= amountOfUnitsOnHold)
            {
                foreach (Tank tank in tanks)
                {
                    ownedUnits.Remove(tank);
                    if (enemyCommandCenters.Any())
                        tank.StartTask(new MoveUnitTask(tank, enemyCommandCenters[UnityEngine.Random.Range(0, enemyCommandCenters.Count)].transform.position));
                    else
                        tank.StartTask(new MoveUnitTask(tank, controllingCommandCenter.transform.position));
                }
                amountOfUnitsOnHold = UnityEngine.Random.Range(5, 15);
            }
        }
    }

    private RtsAction GetProduceTankAction(List<Tank> tanks, List<Building> enemyturrets, WarFactory factory)
    {
        RtsAction actionToPrepare;
        if (enemyturrets.Count * 0.5 > tanks.Count)
            actionToPrepare = factory.GetActions().LastOrDefault(); // add heavy tank
        else
            actionToPrepare = factory.GetActions().FirstOrDefault();

        return actionToPrepare;
    }

    private DesirePriority CalculateDesiresAndDangers()
    {
        // Calculate all desired amounts
        int dozerAmount = ownedUnits.Count(u => u is ConstructionDozer);
        int warFactoryAmount = ownedBuildings.Count(b => b is WarFactory);
        int enemyUnitsAmount = GameManager.Instance.unitManager.GetEnemyUnits(controllingCommandCenter.ownedByTeam).Count();

        // Calculate desires using the desire objects
        desiredConstructors =   constructorDesireObject.CalculateDesire(dozerAmount + bullDozersInQueue, 1);
        desiredFactories =      factoryDesireObject.CalculateDesire(warFactoryAmount, dozerAmount);
        commandCenterDanger =   commandCenterDangerObject.CalculateDesire(GetEnemiesInProximity().Count);
        desiredOffensiveUnits = offensiveUnitDesireObject.CalculateDesire(warFactoryAmount, ownedUnits.Count);
        desiredDefenses =       defensiveUnitDesireObject.CalculateDesire(dozerAmount, GetTurrets(controllingCommandCenter.ownedByTeam).Count, enemyUnitsAmount);
        desiredSupplyCenters =  supplyResourceDesireObject.CalculateSupplyCenterDesire(ownedBuildings.Count(b => b is SupplyCenter));
        desiredSupplyTrucks = 0;//supplyResourceDesireObject.CalculateSupplyTruckDesire(ownedUnits.Count(t => t is SupplyTruck));

        desiredOffensiveUnits *= loadedAggresiveness;
        desiredDefenses *= loadedDefensiveness;

        desiredConstructors =   Mathf.Clamp(desiredConstructors, 0, 1);
        desiredFactories =      Mathf.Clamp(desiredFactories, 0, 1);
        commandCenterDanger =   Mathf.Clamp(commandCenterDanger, 0, 1);
        desiredOffensiveUnits = Mathf.Clamp(desiredOffensiveUnits, 0, 1);
        desiredDefenses =       Mathf.Clamp(desiredDefenses, 0, 1);
        desiredSupplyTrucks =   Mathf.Clamp(desiredSupplyTrucks, 0, 1);
        desiredSupplyCenters =  Mathf.Clamp(desiredSupplyCenters, 0, 1);

        // Determine the highest desire and return the corresponding DesirePriority
        float maxDesire = Mathf.Max(desiredConstructors, desiredOffensiveUnits, desiredFactories, commandCenterDanger, desiredDefenses, desiredSupplyTrucks, desiredSupplyCenters);

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
        else if (maxDesire == desiredDefenses)
            return DesirePriority.Defend;
        else if (maxDesire == desiredSupplyCenters)
            return DesirePriority.SupplyCenter;
        else if (maxDesire == desiredSupplyTrucks)
            return DesirePriority.SupplyTruck;


        return DesirePriority.None; // Default case
    }

    private List<Building> GetTurrets(Team team)
    {
        List<Building> turrets = new();
        foreach (var building in GameManager.Instance.buildingManager.AllBuildings)
        {
            if (building.ownedByTeam == team && building is TurretBuilding)
            {
                turrets.Add(building);
            }
        }
        return turrets;
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

        if (controllingCommandCenter != null)
        {
            Collider[] colliders;

            colliders = Physics.OverlapSphere(controllingCommandCenter.transform.position, dangerDetectionRadius);

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
        }

        return enemyObjects;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(controllingCommandCenter.transform.position, dangerDetectionRadius);
    }

    //private SupplyDock GetClostestSupplyDock()
    //{
    //    var teamManager = GameManager.Instance.teamManager;
    //    List<ITeamable> enemyObjects = new List<ITeamable>();

    //    if (controllingCommandCenter != null)
    //    {
    //        Collider[] colliders;

    //        colliders = Physics.OverlapSphere(controllingCommandCenter.transform.position, 100);
    //        for (int i = 0; i < colliders.Length; i++)
    //        {
    //            SupplyDock dock = colliders[i].gameObject.GetComponent<SupplyDock>();
    //            if (dock != null) return dock;
    //        }
            
    //    }
    //    return null;
    //}
}
