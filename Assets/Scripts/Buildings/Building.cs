using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Building : MonoBehaviour, ISelectable
{
    public List<RtsBuildingAction> buildingActions = new(8); //These are empty RTS action slots

    [SerializeField] protected Transform selectableHighlightParent;
    
    public Transform unitSpawnPoint; //The place units will spawn from

    protected GameObject instantiatedSelectionObject;

    public Team ownedByTeam;

    public abstract void Deselect();
    public abstract GameObject GetGameObject();
    public abstract void Select();    
}
