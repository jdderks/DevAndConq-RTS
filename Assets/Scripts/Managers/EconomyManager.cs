using UnityEngine;
using System.Collections.Generic;
using System;

public class EconomyManager : MonoBehaviour
{
    public List<Economy> economies = new();




    public Economy GetEconomy(TeamColour colour)
    {
        var commandCenters = GameManager.Instance.teamManager.GetAllCommandCenters();

        //go through command centers and return the economy component where the team is colour
        foreach (var commandCenter in commandCenters)
        {
            if (commandCenter.ownedByTeam.teamByColour == colour)
            {
                return commandCenter.Economy;
            }
        }
        //Debug.LogError($"There's no command centers with an economy of {colour.ToString()}");
        return null;
    }
}