using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionQueuePanel : MonoBehaviour
{
    [SerializeField] private Transform panelParent;
    public List<ActionQueuePanelItem> panelItems = new();
    private bool panelIsActive = false;
    private ActionQueue focussedActionQueue;
    public bool PanelIsActive { get => panelIsActive; set => panelIsActive = value; }

    public void TotalPanelReset(ActionQueue queue)
    {
        if (queue == null)
            queue = focussedActionQueue;

        CloseActionQueueItems();
        SetActionQueue(queue);
    }

    public void SetActionQueue(ActionQueue queue)
    {
        focussedActionQueue = queue;
        InstantiateQueue();
    }

    private void InstantiateQueue()
    {
        panelIsActive = true;
        foreach (var item in focussedActionQueue.Items)
        {
            AddNewQueuePanelItem();
        }
    }

    private void AddNewQueuePanelItem()
    {
        GameObject panelGameObject = Instantiate(GameManager.Instance.Settings.uiPanelSettings.queueItemPrefab, panelParent);
        ActionQueuePanelItem queueItem = panelGameObject.GetComponent<ActionQueuePanelItem>();
        panelItems.Add(queueItem);
    }

    public void CloseActionQueueItems()
    {
        foreach (var panel in panelItems)
        {
            //maybe remove click events later as well
            Destroy(panel.gameObject);
        }
        panelItems = new();
        panelIsActive = false;
    }

    private void Update()
    {
        UpdatePanel();
    }

    public void UpdatePanel()
    {
        if (!panelIsActive) return;

        if (focussedActionQueue.Items.Count > 0)
        {
            float progress = focussedActionQueue.Items[0].RemainingTime / focussedActionQueue.Items[0].WaitForSeconds;
            float clampedProgress = Mathf.Clamp01(progress);
            float invertedProgress = 1f - clampedProgress;

            if (panelItems.Count > 0)
            {
                panelItems[0].frontImage.fillAmount = invertedProgress;
            }
        }

        if (focussedActionQueue.IsOutDated)
        {
            TotalPanelReset(queue: focussedActionQueue);
            focussedActionQueue.IsOutDated = false;
        }
    }
}
