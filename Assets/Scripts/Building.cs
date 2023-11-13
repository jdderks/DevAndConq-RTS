using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Building : MonoBehaviour, ISelectable
{
    public List<BuildingAction> buildingActions = new(8);

    public abstract void Deselect();
    public abstract GameObject GetGameObject();
    public abstract void Select();
}
