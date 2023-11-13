using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BuildingAction
{
    public Action Action { get; set; }
    public float actionCooldown;
    public Sprite sprite;
}
