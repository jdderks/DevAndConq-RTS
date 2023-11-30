using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable, CreateAssetMenu(fileName = "Settings", menuName = "Settings/RtsActions", order = 3)]

public class RtsActionSettings : ScriptableObject
{
    public PanelInfoScriptableObject bullDozerPanelInfo;
}
