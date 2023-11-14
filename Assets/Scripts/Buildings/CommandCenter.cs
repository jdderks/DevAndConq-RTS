using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandCenter : Building
{



    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void Deselect()
    {
        Destroy(instantiatedSelectionObject);
    }

    public override GameObject GetGameObject()
    {
        return gameObject;
    }

    public override void Select()
    {
        Assert.IsNotNull(selectableHighlightParent, "Parent object not set in prefab.");
        instantiatedSelectionObject = Instantiate(GameManager.Instance.Settings.ModelSettings.unitSelectionHighlightGameObject, selectableHighlightParent);
    }

}
