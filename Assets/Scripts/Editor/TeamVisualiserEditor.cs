using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class TeamVisualiserEditor : EditorWindow
{
    

    [MenuItem("Window/Team Visualiser")]
    public static void ShowWindow()
    {
        GetWindow<TeamVisualiserEditor>("Team Visualiser");
    }

    private void OnGUI()
    {
        //if not in playmode return early
        if (!Application.isPlaying)
        {
            GUILayout.Label("Please enter play mode to use this tool");
            return;
        }


        GameManager gameManager = GameManager.Instance;
        foreach (Team item in gameManager.teamManager.teams)
        {
            Team team = item;
            var money = gameManager.economyManager.GetEconomy(team.teamByColour).CurrentAmountOfMoney;
            GUILayout.Label("Team " + team.teamByColour.ToString() + " has " + money + " amount of money.");
        }
    }
}
