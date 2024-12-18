using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UnitManager : Manager
{
    private List<Unit> units = new List<Unit>();

    public List<Unit> Units { get => units; set => units = value; }

    public void Update()
    {
        foreach (var unit in units)
        {
            if (unit.CurrentTask == null || unit.CurrentTask.TaskState == TaskState.Completed)
                unit.StartTask(new IdleTask(unit));
        }
    }

    public void RegisterUnit(Unit unit)
    {
        Units.Add(unit);
    }

    public void UnRegisterUnit(Unit unit)
    {
        Units.Remove(unit);
    }

    public void AddSelected(GameObject go)
    {
        ITeamable gameObjectTeamable = go.GetComponent<ITeamable>();
        TeamColour currentControllingTeam = GameManager.Instance.teamManager.TeamCurrentlyControlling.teamByColour;
        if (gameObjectTeamable != null)
            if (currentControllingTeam != gameObjectTeamable.GetTeam())
                return;

        var building = go.GetComponent<Building>();
        if (building != null)
        {
            if (building.Interactable)
            {
                GameManager.Instance.SelectableCollection.AddSelected(go);
            }
        }
        else
        {
            GameManager.Instance.SelectableCollection.AddSelected(go);
        }
    }

    public void DeselectAll()
    {
        GameManager.Instance.SelectableCollection.deselectAll();
    }

    public List<Unit> GetEnemyUnits(Team team)
    {
        List<Unit> enemyUnits = new();
        foreach (var unit in units)
        {
            if (team.enemies.Contains(unit.OwnedByTeam.teamByColour))
            {
                enemyUnits.Add(unit);
            }
        }
        return enemyUnits;
    }

    //public void SetTask(Unit unit, UnitTask task)
    //{

    //}
    //public void SetTask(List<Unit> units, UnitTask task)
    //{

    //}

    //internal List<Unit> GetUnits()
    //{
    //    return GameManager.Instance.SelectableCollection.GetSelectedUnits();
    //}
}