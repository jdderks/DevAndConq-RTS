using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class TurretBuilding : Building, IAIControllable
{
    [SerializeField] private Turret turret;


    public void AIUpdate()
    {
        var enemies = GetEnemiesInProximity();
        if (enemies.Count > 0)
            if (enemies[0] is IDamageable damageable)
                if (turret != null)
                    turret.Attack(damageable);
    }

    protected List<ITeamable> GetEnemiesInProximity()
    {
        var teamManager = GameManager.Instance.teamManager;
        List<GameObject> teamableObjectsInProximity = GetUnitsAndBuildingsInProximity();

        List<TeamByColour> enemyTeams = teamManager.GetEnemyTeams(ownedByTeam);

        List<string> enemyTags = new List<string>();
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

    public override void Deselect()
    {
        throw new System.NotImplementedException();
    }

    public override GameObject GetGameObject()
    {
        throw new System.NotImplementedException();
    }

    public override void Select()
    {
        throw new System.NotImplementedException();
    }
}
