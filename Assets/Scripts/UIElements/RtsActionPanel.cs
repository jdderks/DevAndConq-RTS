using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
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

            ISelectable origin = action.GetOrigin(); // = action.GetOrigin() as Building;

            panel.SetPanelItemInfo(
                image: actionInfo.image,
                buttonText: actionInfo.panelText,
                textCost: actionInfo.cost.ToString(),
                actionDelay: actionInfo.actionDelay
            );

            void AddActionQueueItem()
            {
                switch (action.GetActionType())
                {
                    case RTSActionType.None:
                        break;
                    case RTSActionType.SpawnUnit:
                        Assert.IsNotNull(origin.GetActionQueue(), "Origin doesn't have an action queue set!");
                        RtsQueueAction actionQueue = origin.GetActionQueue().AddToActionQueue(action);
                        break;
                    case RTSActionType.BuildStructure:
                        //Debug.Log("Origin: " + origin);
                        //Debug.Log("Prefab:" + actionInfo.actionPrefab);
                        GameManager.Instance.buildingManager.EnterBuildingPlacementMode(origin as Unit, actionInfo.actionPrefab);
                        break;
                    case RTSActionType.Upgrade:
                        break;
                    default:
                        break;
                }
            }

            panel.SetButtonInteraction(AddActionQueueItem);
        }
    }
}
