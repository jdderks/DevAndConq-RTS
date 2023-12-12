using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Building : MonoBehaviour, ISelectable
{
    public List<RtsAction> rtsBuildingActions = new(); //These are empty RTS action slots
    public ActionQueue actionQueue = new ActionQueue(); //This could be a Queue<> but I'd like items to be able to be removed from the center.


    [SerializeField] protected Transform selectableHighlightParent;
    
    public Transform unitSpawnPoint; //The place units will spawn from

    protected GameObject instantiatedSelectionObject;

    public Team ownedByTeam;

    public abstract void Deselect();
    public abstract GameObject GetGameObject();
    public abstract void Select();    
}