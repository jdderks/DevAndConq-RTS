using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Building : MonoBehaviour, ISelectable
{
    public List<RtsAction> rtsBuildingActions = new(); //These are empty RTS action slots
    public ActionQueue actionQueue = new ActionQueue(); //This could be a Queue<> but I'd like items to be able to be removed from the center.

    [SerializeField] protected Transform selectableHighlightParent;

    [SerializeField] private GameObject visualObjects;
    [SerializeField] private GameObject constructionPlatform;


    private float constructionPercentage = 100f; //assuming building is already constructed
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

    public void SetAsConstructing()
    {
        ConstructionPercentage = 0;
        visualObjects.SetActive(false);
        constructionPlatform.SetActive(true);
    }

    private IEnumerator StartCountdown()
    {
        int count = 0;

        while (count <= 0)
        {
            Debug.Log("Countdown: " + count);
            yield return new WaitForSeconds(constructionDurationInSeconds / 100);
            count--;
        }
    }

    public void Construct(Unit beingConstructedByUnit = null, int speed = 1)
    {

    }

    public void FinishConstruction()
    {
        visualObjects.SetActive(true);
        constructionPlatform.SetActive(false);
    }
}