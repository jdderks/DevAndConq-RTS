using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RTSAction
{
    public abstract void Activate();

    public abstract ISelectable GetOrigin();
}
