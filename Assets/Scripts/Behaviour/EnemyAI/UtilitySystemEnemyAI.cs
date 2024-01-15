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




public class UtilitySystemEnemyAI : MonoBehaviour, IAIControllable, IAIEnemyBase
{
    //Ai Enemy Timings
    private float timer = 0f;
    private float timerIntervalInSeconds = 2f;

    //private Team ownedByTeam;

    private AIBaseState currentBaseState = AIBaseState.None;

    private List<Building> ownedBuildings = new();
    private List<Unit> ownedUnits = new();

    private List<CommandCenter> enemyCommandCenters = new();

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
        int dozerAmount = ownedUnits.Where(u => u is ConstructionDozer).ToList().Count;
        desiredConstructors = constructorDesireObject.CalculateDesire(dozerAmount);

        int warFactoryAmount = ownedBuildings.Where(u => u is WarFactory).ToList().Count;
        desiredFactories = factoryDesireObject.CalculateDesire(warFactoryAmount, dozerAmount);

        commandCenterDanger = commandCenterDangerObject.CalculateDesire(GetEnemiesInProximity().Count);
        


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
