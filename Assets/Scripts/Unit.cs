using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Unit : MonoBehaviour, ISelectableMultiple
{
    [SerializeField] private Transform selectableHighlightParent;
    private GameObject instantiatedObject = null;
    private IMovable movement;

    private UnitTask currentTask;

    public IMovable Movement { get => movement; set => movement = value; }

    private void OnEnable()
    {
        movement = GetComponent<VehicleMovement>(); //TODO; make more generic for all types of vehicles. Maybe dependant on enum
        foreach (Transform t in transform)
        {
            if (t.name == "Selection")
            {
                selectableHighlightParent = t;
            }
        }
    }

    public void Select()
    {
        instantiatedObject = GameManager.Instance.Settings.ModelSettings.unitSelectionHighlightGameObject;
        Assert.IsNotNull(instantiatedObject, "Object not assigned in the Game Settings or error in Unit.cs.");
        Assert.IsNotNull(selectableHighlightParent, "Parent object not set in prefab.");
        instantiatedObject = Instantiate(instantiatedObject, selectableHighlightParent);
    }

    public void Deselect()
    {
        Destroy(instantiatedObject);
    }



    public GameObject GetObject()
    {
        return gameObject;
    }

    public void StartTask(UnitTask task)
    {
        currentTask = task;
        task.Begin();
    }
}
