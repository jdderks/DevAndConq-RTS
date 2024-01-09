using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public enum ConstructionState
{
    None = -1,
    ConstructionPaused = 0,
    UnderConstruction = 1,
    FinishedConstruction = 2
}

public abstract class Building : MonoBehaviour, ISelectable
{
    [HorizontalLine, Header("Team: "), SerializeField] public TeamAppearanceScriptableObject ownedByTeam;

    public List<RtsAction> rtsBuildingActions = new(); //These are empty RTS action slots
    public ActionQueue actionQueue = new ActionQueue(); //This could be a Queue<> but I'd like items to be able to be removed from the center.

    private ConstructionState constructionState = ConstructionState.None;

    [HorizontalLine, SerializeField] protected Transform selectableHighlightParent;

    [SerializeField] private GameObject visualObjects;
    [SerializeField] private GameObject constructionPlatform;
    [SerializeField] private bool interactable = false;
    [SerializeField] TextMeshProUGUI constructionPercentageText;


    private Unit unitBeingConstructedBy = null;

    private float constructionPercentage = 100f; //assuming building is already constructed
    private float constructionDurationInSeconds = 20f;

    public Transform unitSpawnPoint; //The place units will spawn from

    protected GameObject instantiatedSelectionObject;


    public float ConstructionPercentage { get => constructionPercentage; set => constructionPercentage = value; }
    public float ConstructionDurationInSeconds { get => constructionDurationInSeconds; set => constructionDurationInSeconds = value; }
    public bool Interactable { get => interactable; set => interactable = value; }

    public void Update()
    {
        if (actionQueue != null)
            actionQueue.Update();

        if (constructionState == ConstructionState.UnderConstruction)
            Construct();
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

    public void StartConstruction(Unit unit = null, int speed = 1)
    {
        unitBeingConstructedBy = unit;
        constructionState = ConstructionState.UnderConstruction;
    }

    public void Construct()
    {
        if (constructionPercentage < 100f)
        {
            float increment = (Time.deltaTime / constructionDurationInSeconds) * unitBeingConstructedBy.ConstructionMultiplier * 100f;
            constructionPercentage += increment;
            constructionPercentage = Mathf.Clamp(constructionPercentage, 0f, 100f);
            constructionPercentageText.text = ConstructionPercentage.ToString("N0") + "%";
        }
        if (constructionPercentage >= 100f)
        {
            FinishConstruction();
        }
    }

    public void StopConstruction()
    {
        constructionState = ConstructionState.ConstructionPaused;
    }

    [Button("Set as constructing")]
    public void ResetConstruction()
    {
        Interactable = false;
        ConstructionPercentage = 0;
        visualObjects.SetActive(false);
        constructionPlatform.SetActive(true);
        constructionState = ConstructionState.ConstructionPaused;
    }

    [Button("Finish Construction")]
    public void FinishConstruction()
    {
        Interactable = true;
        visualObjects.SetActive(true);
        constructionPlatform.SetActive(false);
        constructionState = ConstructionState.FinishedConstruction;
        if (unitBeingConstructedBy == null) return;
        if (unitBeingConstructedBy.CurrentTask is ConstructionTask constructionTask)
        {
            constructionTask.Finish();
        }
        else if (unitBeingConstructedBy.CurrentTask is SequenceTask seqTask && seqTask.GetCurrentTask() is ConstructionTask)
        {
            ConstructionTask constrTask = seqTask.GetCurrentTask() as ConstructionTask;
            constrTask.Finish();
        }

    }
}