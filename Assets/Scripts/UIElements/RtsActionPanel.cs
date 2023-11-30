using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RtsActionPanel : MonoBehaviour
{
    List<PanelItem> panels = new();

    public void UpdatePanels(List<RtsAction> actions)
    {
        if (panels.Count >= 8 || panels.Count == 0)
        {
            Debug.LogWarning("Can not add 0 or more than 7 (8 including 0) panels!");
            return; //Early return
        }

        foreach (var action in actions)
        {
            CreatePanelItem(action);
        }
    }

    public void CreatePanelItem(RtsAction action)
    {
        PanelItem item = new();

        //item.SetPanelItemInfo(action.GetSprite(), action.GetName());


    }
}
