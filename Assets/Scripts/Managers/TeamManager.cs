using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum TeamByColour
{
    None = -1,
    Red = 0,
    Blue = 1,
    White = 2
}

public class TeamManager : MonoBehaviour
{
    [ReadOnly, SerializeField] private Team teamCurrentlyControlling;

    [Button("Set Red team Controlling")]
    public void SetRedTeamControlling()
    {
        teamCurrentlyControlling = teams.Where(team => team.teamByColour == TeamByColour.Red).FirstOrDefault();

    }

    [Button("Set Blue team Controlling")]
    public void SetBlueTeamControlling()
    {
        teamCurrentlyControlling = teams.Where(team => team.teamByColour == TeamByColour.Blue).FirstOrDefault();

    }

    public List<Team> teams = new();

    public List<TeamByColour> GetEnemyTeams(Team team)
    {
        return team.enemies;
    }

    public Team GetTeamByColour(TeamByColour teamByColour)
    {
        foreach (Team team in teams)
        {
            if (team.teamByColour == teamByColour)
            {
                return team;
            }
        }

        return null;
    }
}
