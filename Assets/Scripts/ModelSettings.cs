using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable, CreateAssetMenu(fileName = "Settings", menuName = "Settings/Models", order = 2)]

public class ModelSettings : ScriptableObject
{
    public GameObject unitSelectionHighlightGameObject;
}
