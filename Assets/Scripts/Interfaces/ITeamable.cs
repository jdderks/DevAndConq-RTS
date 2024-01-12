using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITeamable
{
    public void SetTeam(TeamByColour teamByColour);

    public GameObject GetGameObject();
}