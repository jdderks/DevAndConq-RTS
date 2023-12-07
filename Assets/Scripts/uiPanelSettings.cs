using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UIPanelSettings", menuName = "Settings/uiPanels")]
public class uiPanelSettings : ScriptableObject
{
    public GameObject panelItemPrefab;
    public GameObject queueItemPrefab;
}
