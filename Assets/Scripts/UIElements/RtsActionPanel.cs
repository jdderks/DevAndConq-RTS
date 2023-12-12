using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RtsActionPanel : MonoBehaviour
{
    [SerializeField] private Transform panelParent;
    List<ActionPanelItem> panels = new();


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
            GameObject panelGameObject = Instantiate(GameManager.Instance.Settings.uiPanelSettings.panelItemPrefab, panelParent);
            ActionPanelItem panel = panelGameObject.GetComponent<ActionPanelItem>();

            if (action is CreateUnitRTSAction)
            {
                var CreateUnitAction = action as CreateUnitRTSAction;
            }

            PanelInfoScriptableObject actionInfo = action.GetPanelInfo();

            panels.Add(panel);
            Building origin = action.GetOrigin() as Building;

            panel.SetPanelItemInfo(
                image: actionInfo.image,
                buttonText: actionInfo.panelText,
                textCost: actionInfo.cost.ToString(),
                actionDelay: actionInfo.constructionTime
            );

            

            void AddActionQueueItem()
            {
                origin.actionQueue.AddToActionQueue(action);
            }

            panel.SetButtonInteraction(AddActionQueueItem);
        }
    }
}
