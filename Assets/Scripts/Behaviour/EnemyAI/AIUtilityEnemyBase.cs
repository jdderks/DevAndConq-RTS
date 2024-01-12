using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public enum AIBaseState
{
    None = -1,
    Passive = 0,
    PreparingAttack = 1,
    MakingDefences = 2,
    Attack = 3
}




public class AIUtilityEnemyBase : MonoBehaviour, IAIControllable, IAIEnemyBase
{
    //Ai Enemy Timings
    private float timer = 0f;
    private float timerIntervalInSeconds = 2f;

    private Team ownedByTeam;

    private AIBaseState currentBaseState = AIBaseState.None;

    private List<Building> buildings = new();
    private List<Unit> units = new();

    private List<CommandCenter> enemyCommandCenters = new();

    [SerializeField] private BuildingPositioner buildingPositioner;




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
        throw new System.NotImplementedException();
    }
}
