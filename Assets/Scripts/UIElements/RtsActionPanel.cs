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
            panel.RemoveEvents();
            Destroy(panel.gameObject);
        }
        panels = new();

        foreach (var action in actions)
        {
            Debug.Log("EE");
            GameObject panelGameObject = Instantiate(GameManager.Instance.Settings.uiPanelSettings.panelItemPrefab, panelParent);
            PanelItem panel = panelGameObject.GetComponent<PanelItem>();
            if (action is CreateUnitRTSAction)
            {
                var CreateUnitAction = action as CreateUnitRTSAction;

                var originBuilding = CreateUnitAction.GetBuilding();//.GetGameObject().GetComponent<Building>();

            }


            PanelInfoScriptableObject actionInfo = action.GetPanelInfo();

            panels.Add(panel);

            panel.SetPanelItemInfo(
                image: actionInfo.image, 
                buttonText: actionInfo.panelText,
                textCost: actionInfo.cost.ToString(),
                action.Activate
            );
        }
    }
}
