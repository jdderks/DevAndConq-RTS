using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour, ISelectableMultiple
{
    private GameObject selectableHighlightParent;
    [SerializeField] private GameObject cone = null;

    private void OnEnable()
    {
        foreach (Transform t in transform)
        {
            if (t.name == "Selection")
            {
                selectableHighlightParent = t.gameObject;
            }
        }
    }

    public void Select()
    {
        cone = Instantiate(selectableHighlightParent, cone.transform);
    }

    public void Deselect()
    {
        Destroy(cone);
    }

    public GameObject GetObject()
    {
        return gameObject;
    }

}
