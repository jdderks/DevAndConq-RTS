using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable, CreateAssetMenu(fileName = "Settings", menuName = "Settings/Models", order = 2)]
public class ModelSettings : ScriptableObject
{
    public GameObject unitSelectionHighlightGameObject;
    public GameObject terrainInteractionObject;

    [Header("Units")]
    public GameObject constructionDozer;
    public GameObject lightTank;
    public GameObject heavyTank;

    public GameObject GetUnitByEnum(UnitToSpawn unit)
    {
        switch (unit)
        {
            case UnitToSpawn.None: return null;
            case UnitToSpawn.Bulldozer: return constructionDozer;
            case UnitToSpawn.LightTank: return lightTank;
            case UnitToSpawn.HeavyTank: return heavyTank;
            default: return null;
        }
    }
}
