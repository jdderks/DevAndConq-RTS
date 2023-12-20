using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SelectableCollection : MonoBehaviour
{
    public Dictionary<int, ISelectable> selectedTable = new Dictionary<int, ISelectable>();

    public ISelectable SingleSelectable;

    public void AddSelected(GameObject go)
    {
        int id = go.GetInstanceID();

        if (!(selectedTable.ContainsKey(id)))
        {
            if (go.GetComponent<Unit>() != null)
            {
                var unit = go.GetComponent<Unit>();
                if (unit is ISelectableMultiple)
                {
                    unit.Select();
                    selectedTable.Add(id, go.GetComponent<Unit>());
                }
                else if (unit is ISelectable)
                {
                    if (!selectedTable.Values.Any(obj => obj is ISelectableMultiple))
                    {
                        unit.Select();
                        selectedTable.Add(id, go.GetComponent<Unit>());
                    }
                }
            }
            else if (go.GetComponent<Building>() != null)
            {
                var building = go.GetComponent<Building>();
                if (building is ISelectable)
                {
                    if (!selectedTable.Values.Any(obj => obj is ISelectableMultiple))
                    {
                        building.Select();
                        selectedTable.Add(id, go.GetComponent<Building>());
                    }
                }
            }
        }
        UpdateUnitUI();
    }


    public void deselect(int id)
    {
        selectedTable[id].Deselect();
        selectedTable.Remove(id);
        UpdateUnitUI();
    }

    public void deselectAll()
    {
        foreach (KeyValuePair<int, ISelectable> pair in selectedTable)
        {
            if (pair.Value != null)
            {
                selectedTable[pair.Key].Deselect();
            }
        }
        selectedTable.Clear();
        UpdateUnitUI();
    }

    public IEnumerable<ISelectable> GetSelectables()
    {
        return selectedTable.Values;
    }

    public List<Unit> GetSelectedUnits()
    {
        return selectedTable.Values.
            Select(item => item.GetGameObject().GetComponent<Unit>()).
            Where(unit => unit != null).ToList();
    }


    public Unit GetUnitFromSelectable(ISelectable selectable)
    {
        if (selectable.GetGameObject().GetComponent<Unit>() != null)
            return selectable.GetGameObject().GetComponent<Unit>();
        else
            return null;
    }

    public void UpdateUnitUI()
    {
        GameManager.Instance.uiManager.UpdateInfoPanelValues();
    }

    internal ISelectable GetSingleSelectable()
    {
        List<ISelectable> selectables = selectedTable.Values
            .Select(item => item.GetGameObject().GetComponent<ISelectable>())
            .Where(unit => unit != null)
            .ToList();

        if (selectables.Count == 1)
        {
            return selectables[0];
        }
        else
        {
            return null;
        }
    }
}