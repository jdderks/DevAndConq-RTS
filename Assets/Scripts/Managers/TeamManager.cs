using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum TeamColour
{
    None = -1,
    Red = 0,
    Blue = 1,
    White = 2
}

public class TeamManager : Manager
{
    [ReadOnly, SerializeField] private Team teamCurrentlyControlling;

    [Button("Set Red team Controlling")]
    public void SetRedTeamControlling()
    {
        TeamCurrentlyControlling = teams.Where(team => team.teamByColour == TeamColour.Red).FirstOrDefault();

    }

    [Button("Set Blue team Controlling")]
    public void SetBlueTeamControlling()
    {
        TeamCurrentlyControlling = teams.Where(team => team.teamByColour == TeamColour.Blue).FirstOrDefault();

    }

    public List<Team> teams = new();

    public Team TeamCurrentlyControlling 
    { 
        get => teamCurrentlyControlling; 
        private set => teamCurrentlyControlling = value; 
    }

    public List<TeamColour> GetEnemyTeams(Team team)
    {
        return team.enemies;
    }

    public Team GetTeamByColour(TeamColour teamByColour)
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

    public IEnumerable<CommandCenter> GetAllCommandCenters()
    {
        return FindObjectsByType<CommandCenter>(FindObjectsSortMode.None);
    }

}
