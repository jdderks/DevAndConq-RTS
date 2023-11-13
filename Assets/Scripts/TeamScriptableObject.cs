using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Settings", menuName = "Team/New", order = 1)]
public class TeamScriptableObject : ScriptableObject
{
    public int teamID;
    public string teamName;
    public bool isPlayerControlled;
    public Color teamColour;
}
