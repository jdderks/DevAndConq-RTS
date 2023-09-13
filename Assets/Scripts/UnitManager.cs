using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Diagnostics;
using NaughtyAttributes;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.AI;


public class UnitManager : MonoBehaviour
{
    [SerializeField] private SelectableCollection selectableCollection;

    public SelectableCollection SelectableCollection { get => selectableCollection; set => selectableCollection = value; }

    public void AddSelected(GameObject gameobject)
    {
        SelectableCollection.addSelected(gameObject);
    }

    public void DeselectAll() 
    { 
        SelectableCollection.deselectAll();
    }


    public void SetTask(Unit unit, UnitTask task)
    {

    }
    public void SetTask(List<Unit> units, UnitTask task)
    {

    }
}