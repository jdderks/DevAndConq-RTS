using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UI", menuName = "PanelInfo/New", order = 1)]

public class PanelInfoScriptableObject : ScriptableObject
{
    public int cost;
    public string panelText;
    public Sprite image;
}
