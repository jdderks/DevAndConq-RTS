using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum RTSActionType
{
    None = 0,
    SpawnUnit = 1,
    BuildStructure = 2,
    Upgrade = 3
}

[CreateAssetMenu(fileName = "UI", menuName = "PanelInfo/New", order = 1)]

public class PanelInfoScriptableObject : ScriptableObject
{
    public int cost;
    public string panelText;
    public Image image;
    public RTSActionType actionType;
    public GameObject actionPrefab;
    public float actionDelay;
    //public UnitToSpawn unit;
}
