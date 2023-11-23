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
    public SelectionManager unitSelection;
    public MovementManager movementManager;
    public InputManager inputManager;
    public UiManager uiManager;

    [SerializeField] private SelectableCollection selectableCollection;
    public SelectableCollection SelectableCollection { get => selectableCollection; set => selectableCollection = value; }
}
