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
    [ReadOnly, SerializeField] private TeamAppearanceScriptableObject teamCurrentlyControlling;

    [Button("Set Red team Controlling")]
    public void SetRedTeamControlling()
    {
        teamCurrentlyControlling = teams.Select(t => t.teamObject)
                                        .Where(teamObj => teamObj.teamColour == TeamByColour.Red)
                                        .FirstOrDefault();
    }

    [Button("Set Blue team Controlling")]
    public void SetBlueTeamControlling()
    {
        teamCurrentlyControlling = teams.Select(t => t.teamObject)
                                        .Where(teamObj => teamObj.teamColour == TeamByColour.Blue)
                                        .FirstOrDefault();
    }

    public List<Team> teams = new();

    public List<Team> GetEnemyTeams(Team team)
    {
        List<Team> matchingTeams = new List<Team>();

        foreach (Team t in teams)
        {
            bool hasEnemy = false;
            foreach (var enemy in t.enemies)
            {
                if (enemy.Equals(team.teamObject.teamColour))
                {
                    hasEnemy = true;
                    break;
                }
            }

            if (hasEnemy)
            {
                matchingTeams.Add(t);
            }
        }

        return matchingTeams;
    }

    public Team GetTeamByColour(TeamByColour teamByColour)
    {
        foreach (Team team in teams)
        {
            if (team.teamObject.teamColour == teamByColour)
            {
                return team;
            }
        }

        return null;
    }

}
