using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Team
{
    public TeamScriptableObject teamStyle;

    public List<Team> allies;
    public List<Team> enemies;

    public bool playerControlled = true;
}
