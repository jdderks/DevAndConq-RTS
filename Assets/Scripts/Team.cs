using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Team
{
    public string teamTagName;
    public int teamID;
    public TeamByColour teamByColour;
    public Color colour;
    //public List<TeamByColour> allies;
    public List<TeamByColour> enemies;
}
