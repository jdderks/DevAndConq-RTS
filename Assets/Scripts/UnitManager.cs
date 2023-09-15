using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UnitManager : MonoBehaviour
{
    public List<Unit> units = new List<Unit>();

    public void RegisterUnit(Unit unit)
    {
        units.Add(unit);
    }

    public void UnRegisterUnit(Unit unit)
    {
        units.Remove(unit);
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