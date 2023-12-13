using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BuildingToConstruct
{
    None = 0,
    CommandCenter = 1,
    WarFactory = 2
}

public class ConstructBuildingScriptableObject : ScriptableObject
{
    public int cost;
    public string panelText;
    public Image image;
    public BuildingToConstruct buildingToConstruct;

}
