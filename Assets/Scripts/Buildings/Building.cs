using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

public abstract class Building : MonoBehaviour, ISelectable, ITeamable, IDamageable
{
    [ProgressBar("Building Health", maxValue: 500), SerializeField] private float Health = 500;
    public TeamColour colourEnum;
    [ReadOnly, HorizontalLine, Header("Team: "), SerializeField] public Team ownedByTeam;
    public List<RtsAction> rtsBuildingActions = new(); //These are empty RTS action slots
    public ActionQueue actionQueue = new ActionQueue(); //This could be a Queue<> but I'd like items to be able to be removed from the center.

    public System.Action OnConstructionFinished;

    private ConstructionState constructionState = ConstructionState.None;

    [HorizontalLine, SerializeField] protected Transform selectableHighlightParent;

    [SerializeField] protected bool isInstantiated = false;
    [SerializeField] protected GameObject visualObject;
    [SerializeField] private GameObject constructionPlatform;
    [SerializeField] private bool interactable = false;
    [SerializeField] TextMeshProUGUI constructionPercentageText;

    private Unit unitBeingConstructedBy = null;

    private float constructionPercentage = 100f; //assuming building is already constructed
    [SerializeField] private float constructionDurationInSeconds = 20f;

    public Transform unitSpawnPoint; //The place units will spawn from

    protected GameObject instantiatedSelectionObject;


    public float ConstructionPercentage { get => constructionPercentage; set => constructionPercentage = value; }
    public float ConstructionDurationInSeconds { get => constructionDurationInSeconds; set => constructionDurationInSeconds = value; }
    public bool Interactable { get => interactable; set => interactable = value; }


    private void Start()
    {
        GameManager.Instance.buildingManager.SubscribeBuilding(this);
    }

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

    public abstract bool UnitInteract(Unit unit);


    public List<RtsAction> GetActions()
    {
        return rtsBuildingActions;
    }

    public virtual void SetTeam(TeamColour teamByColour)
    {
        ownedByTeam = GameManager.Instance.teamManager.GetTeamByColour(teamByColour);
        //Renderer renderer = visualObject.GetComponentInChildren<Renderer>();
        //Material teamColourMaterial = renderer.materials[1];
        //teamColourMaterial.color = ownedByTeam.colour;
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
        visualObject.SetActive(false);
        constructionPlatform.SetActive(true);
        constructionState = ConstructionState.ConstructionPaused;
    }

    [Button("Finish Construction")]
    public void FinishConstruction()
    {
        Interactable = true;
        visualObject.SetActive(true);
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
        OnConstructionFinished?.Invoke();
    }

    public void TakeDamage(float amount)
    {
        Health -= amount;
        if (Health < 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        Destroy(gameObject);
    }

    public TeamColour GetTeam()
    {
        return ownedByTeam.teamByColour;
    }

    private void OnDestroy()
    {
        GameManager.Instance.buildingManager.UnsubscribeBuilding(this);
    }
}