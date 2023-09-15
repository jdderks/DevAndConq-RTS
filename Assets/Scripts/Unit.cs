using NaughtyAttributes;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum UnitType
{
    Humanoid = 0,    //Infantry unit
    Quadricycle = 1, //For all car-alikes
    TwoWheeler = 2,  //For bicycles and motorcycles.
    AirPlane = 3,    //For jet aircraft which fly with wings.
    VTOL = 4         //For aircraft that can hover and land and take off vertically
}

public class Unit : MonoBehaviour, ISelectableMultiple
{
    [SerializeField] private Transform selectableHighlightParent;
    [SerializeField] private UnitType unitType;

    private bool isMoving;

    private GameObject instantiatedObject = null;

    private UnitTask currentTask;

    public bool IsMoving { get => isMoving; set => isMoving = value; }
    public UnitType UnitType { get => unitType; set => unitType = value; }
    public UnitTask CurrentTask { get => currentTask; set => currentTask = value; }

    private void OnEnable()
    {
        GameManager.Instance.unitManager.RegisterUnit(this);
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
        Assert.IsNotNull(selectableHighlightParent, "Parent object not set in prefab.");
        instantiatedObject = GameManager.Instance.Settings.ModelSettings.unitSelectionHighlightGameObject;
        Assert.IsNotNull(instantiatedObject, "Object not assigned in the Game Settings or error in Unit.cs.");
        instantiatedObject = Instantiate(instantiatedObject, selectableHighlightParent);
    }

    public void Deselect()
    {
        Destroy(instantiatedObject);
    }



    public GameObject GetGameObject()
    {
        return gameObject;
    }

    public void StartTask(UnitTask task)
    {
        CurrentTask = task;
        task.Begin();
    }

    private void UpdateMovement()
    {

    }

    private void Update()
    {
        if (isMoving == true) UpdateMovement();
    }
}
