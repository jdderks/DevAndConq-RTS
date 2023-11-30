using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable, CreateAssetMenu(fileName = "Settings", menuName = "Settings/MainGame", order = 1)]

public class MainGameSettings : ScriptableObject
{
    public ModelSettings ModelSettings;
    public RtsActionSettings rtsActionSettings;
}
