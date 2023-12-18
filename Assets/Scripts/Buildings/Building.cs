using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Building : MonoBehaviour, ISelectable
{
    public List<RtsAction> rtsBuildingActions = new(); //These are empty RTS action slots
    public ActionQueue actionQueue = new ActionQueue(); //This could be a Queue<> but I'd like items to be able to be removed from the center.

    [SerializeField] protected Transform selectableHighlightParent;

    private float constructionPercentage = 100f;
    private float constructionDurationInSeconds = 20f;

    public Transform unitSpawnPoint; //The place units will spawn from

    protected GameObject instantiatedSelectionObject;

    public Team ownedByTeam;

    public float ConstructionPercentage { get => constructionPercentage; set => constructionPercentage = value; }
    public float ConstructionDurationInSeconds { get => constructionDurationInSeconds; set => constructionDurationInSeconds = value; }

    public void Update()
    {
        if (actionQueue != null)
        {
            actionQueue.Update();
        }
    }

    public abstract void Deselect();
    public abstract GameObject GetGameObject();
    public abstract void Select();

    public List<RtsAction> GetActions()
    {
        return rtsBuildingActions;
    }

    public ActionQueue GetActionQueue()
    {
        return actionQueue;
    }
}