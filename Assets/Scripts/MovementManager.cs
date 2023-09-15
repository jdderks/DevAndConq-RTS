using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class MovementManager : MonoBehaviour
{
    [SerializeField] private UnitManager unitManager;

    private SelectableCollection selectableCollection;

    private void Start()
    {
        selectableCollection = GameManager.Instance.SelectableCollection;
    }

    private void Update()
    {
        OnUpdate();
    }


    //Custom update method which can be enabled/disabled
    public void OnUpdate()
    {
        //HandleMovement();
    }

    public void HandleMovement()
    {
        foreach (Unit unit in unitManager.Units)
        {
            if (unit.CurrentTask is MoveUnitTask)
            {
                MoveUnitTask task = unit.CurrentTask as MoveUnitTask;
                if (unit.IsMoving) task.Move();
            }
        }
    }
    /// <summary>
    /// Draw agent movement gizmo'ze
    /// </summary>
    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        foreach (Unit unit in unitManager.Units)
        {
            if (unit.CurrentTask is MoveUnitTask)
            {
                MoveUnitTask task = unit.CurrentTask as MoveUnitTask;
                for (int i = 0; i < task.Path.corners.Length; i++)
                {
                    Gizmos.DrawSphere(task.Path.corners[i], 0.2f);

                    if (i < task.Path.corners.Length - 1)
                    {
                        Gizmos.DrawLine(task.Path.corners[i], task.Path.corners[i + 1]);
                    }
                }

                // Add a debug ray for the forward direction of the agent
                Debug.DrawRay(unit.transform.position, unit.transform.forward * 5f, Color.red);
            }
        }
    }

}
