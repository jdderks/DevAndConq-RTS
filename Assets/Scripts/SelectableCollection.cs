using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SelectableCollection : MonoBehaviour
{
    public Dictionary<int, ISelectable> selectedTable = new Dictionary<int, ISelectable>();

    public void AddSelected(GameObject go)
    {
        int id = go.GetInstanceID();

        if (!(selectedTable.ContainsKey(id)))
        {
            if (go.GetComponent<Unit>() != null)
            {
                var unit = go.GetComponent<Unit>();
                if (unit is ISelectable)
                {
                    unit.Select();
                    selectedTable.Add(id, go.GetComponent<Unit>());
                }
                Debug.Log("Added " + id + " to selected dict " + " (" + go.name + ")");
            } 
            else  if (go.GetComponent<Building>() != null)
            {
                var building = go.GetComponent<Building>();
                if (building is ISelectable)
                {
                    building.Select();
                    selectedTable.Add(id, go.GetComponent<Building>());
                }
                Debug.Log("Added " + id + " to selected dict " + " (" + go.name + ")");

            }
        }

        UpdateUnitUI();
    }

    //public List<ISelectable> GetSelectedObjects()
    //{
    //    List<ISelectable> selectedObjects = new List<ISelectable>();

    //    foreach (var pair in selectedTable)
    //    {
    //        selectedObjects.Add(pair.Value);
    //    }

    //    return selectedObjects;
    //}


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

    public IEnumerable<ISelectable> GetSelectedObjects()
    {
        return selectedTable.Values;
    }

    public IEnumerable<Unit> GetSelectedUnits()
    {
        return selectedTable.Values.
            Select(item => item.GetGameObject().GetComponent<Unit>()).
            Where(unit => unit != null);
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

}