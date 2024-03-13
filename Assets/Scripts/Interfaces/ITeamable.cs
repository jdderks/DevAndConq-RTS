using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITeamable
{
    public void SetTeam(TeamColour teamByColour);
    public TeamColour GetTeam();

    public GameObject GetGameObject();
}
