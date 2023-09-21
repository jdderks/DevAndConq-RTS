using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UnitManager : MonoBehaviour
{
    [ReorderableList] private List<Unit> units = new List<Unit>();

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
        GameManager.Instance.SelectableCollection.addSelected(go);
    }

    public void DeselectAll()
    {
        GameManager.Instance.SelectableCollection.deselectAll();
    }


    public void SetTask(Unit unit, UnitTask task)
    {

    }
    public void SetTask(List<Unit> units, UnitTask task)
    {

    }
}