using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour, ISelectableMultiple
{
    public void Select()
    {
        GetComponent<Renderer>().material.color = Color.red;
    }

    public void Deselect()
    {
        GetComponent<Renderer>().material.color = Color.white;
    }

    public GameObject GetObject()
    {
        return gameObject;
    }

}
