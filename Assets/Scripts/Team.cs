using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Team
{
    public string teamName;
    public int teamID;
    public TeamAppearanceScriptableObject teamObject;
    //public List<TeamByColour> allies;
    public List<TeamByColour> enemies;
}
