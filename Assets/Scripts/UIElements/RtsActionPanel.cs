using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RtsActionPanel : MonoBehaviour
{
    [SerializeField] private Transform panelParent;
    List<PanelItem> panels = new();


    public void UpdatePanels(List<RtsAction> actions)
    {
        foreach (var panel in panels)
        {
            Destroy(panel.gameObject);
        }
        panels = new();

        foreach (var action in actions)
        {
            var panel = Instantiate(GameManager.Instance.Settings.rtsActionSettings.bullDozerPanelInfo, parent: panelParent);
        }
    }

    public void CreatePanelItem(RtsAction action)
    {
        PanelItem item = new();

        //item.SetPanelItemInfo(action.GetSprite(), action.GetName());


    }
}
