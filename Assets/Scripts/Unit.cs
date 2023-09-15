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
    [SerializeField] float thresholdDistance = 0.1f;
    [SerializeField] private float downwardForce = 9.81f; //Controls gravity

    [SerializeField] private float unitSpeed = 5f;

    [SerializeField] private bool taskDebugInfo = true;

    [SerializeField] private Transform selectableHighlightParent;
    [SerializeField] private UnitType unitType;

    private bool isMoving;

    private GameObject instantiatedObject = null;

    private UnitTask currentTask;

    private NavMeshAgent agent;

    public bool IsMoving { get => isMoving; set => isMoving = value; }
    public UnitType UnitType { get => unitType; set => unitType = value; }
    public UnitTask CurrentTask { get => currentTask; set => currentTask = value; }
    public bool TaskDebugInfo { get => taskDebugInfo; set => taskDebugInfo = value; }
    public float UnitSpeed { get => unitSpeed; set => unitSpeed = value; }
    public NavMeshAgent Agent { get => agent; set => agent = value; }

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        Debug.Log(GameManager.Instance);
        Debug.Log(GameManager.Instance.unitManager);
        Debug.Log(this);
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
        //FallToGround();
        if (isMoving == true) UpdateMovement();
    }

    private void FallToGround()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, float.PositiveInfinity, 1 << 8)) //Layer 8 is ground
        {
            float distanceToGround = hit.distance;

            if (distanceToGround > thresholdDistance)
            {
                // Apply falling logic here (e.g., move the object down)
                transform.position -= Vector3.up * Time.deltaTime * downwardForce; // Adjust the falling speed as needed
            }
        }
    }

    [ExecuteInEditMode]
    private void OnDrawGizmos()
    {
        Debug.DrawRay(transform.position, -transform.up * thresholdDistance, Color.red);
    }


}
