using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

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

            ISelectable origin = action.GetOrigin();
            var building = action.GetOrigin() as Building;

            TeamColour actionColor = TeamColour.None;

            if (building)
                actionColor = building.colourEnum;

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
                        /*RtsQueueAction actionQueue = */
                        origin.GetActionQueue().AddToActionQueue(action);
                        break;
                    case RTSActionType.BuildStructure:
                        GameManager.Instance.buildingManager.EnterBuildingPlacementMode(origin as Unit, actionInfo.actionPrefab);
                        Unit unit = origin as Unit;
                        var economy = GameManager.Instance.economyManager.GetEconomy(unit.GetTeam());
                        economy.DecreaseMoney(actionInfo.cost);
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
