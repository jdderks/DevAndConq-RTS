using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "UI", menuName = "PanelInfo/New", order = 1)]

public class PanelInfoScriptableObject : ScriptableObject
{
    public int cost;
    public string panelText;
    public Image image;
    public UnitToSpawn unit;
    public float constructionTime;
}
