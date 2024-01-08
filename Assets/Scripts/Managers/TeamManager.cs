using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamManager : MonoBehaviour
{
    public List<TeamScriptableObject> availableTeams = new();

    public List<Team> teams = new();


    public List<Team> GetEnemyTeams(Team enemiesOfTeam)
    {
        return enemiesOfTeam.enemies;
    }
}
