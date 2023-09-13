using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SelectableCollection : MonoBehaviour
{
    public Dictionary<int, ISelectable> selectedTable = new Dictionary<int, ISelectable>();

    public void addSelected(GameObject go)
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
        }
    }

    public void deselect(int id)
    {
        selectedTable[id].Deselect();
        selectedTable.Remove(id);
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
    }

    public IEnumerable<ISelectable> GetSelectedObjects()
    {
        return selectedTable.Values;
    }

    public IEnumerable<Unit> GetSelectedUnits()
    {
        return selectedTable.Values.
            Select(item => item.GetObject().GetComponent<Unit>()).
            Where(unit => unit != null);
    }

}