using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamManager : MonoBehaviour
{
    public TeamScriptableObject currentTeam = null;

    public List<TeamScriptableObject> teams = new();

    public List<Material> teamMaterials = new();


    private void Start()
    {
        
    }
}
