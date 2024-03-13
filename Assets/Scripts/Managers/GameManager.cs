using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    [Header("Settings")]
    public MainGameSettings Settings;
    [Space]
    [Header("Managers")]
    public UnitManager unitManager;
    public SelectionManager selectionManager;
    public MovementManager movementManager;
    public InputManager inputManager;
    public UiManager uiManager;
    public BuildingManager buildingManager;
    public TeamManager teamManager;
    public PersonalityManager personalityManager;
    public Updater updater;
    public EconomyManager economyManager;


    [SerializeField] private SelectableCollection selectableCollection;
    public SelectableCollection SelectableCollection { get => selectableCollection; set => selectableCollection = value; }

    public List<ActionPanelItem> PanelItems = new();
}
