using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandCenter : Building
{
    CreateUnitRTSAction constructDozerAction = new CreateUnitRTSAction();

    private void Awake()
    {
        constructDozerAction.PanelInfo = GameManager.Instance.Settings.rtsActionSettings.bullDozerPanelInfo;  
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
    public override void Deselect()
    {
        Destroy(instantiatedSelectionObject);
    }

}
