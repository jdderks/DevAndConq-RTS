using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISelectable
{
    public void Select();
    public void Deselect();
    public GameObject GetGameObject();

    public List<RtsAction> GetActions();

    public ActionQueue GetActionQueue();
}
