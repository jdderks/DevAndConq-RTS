using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionQueuePanel : MonoBehaviour
{
    [SerializeField] private Transform panelParent;
    public List<ActionQueuePanelItem> panelItems = new();

    public void SetActionQueueItems(ActionQueue queue)
    {
        foreach (var panel in panelItems)
        {
            //maybe remove click events later as well
            Destroy(panel.gameObject);
        }
        panelItems = new();

        foreach (var item in queue.Items)
        {
            GameObject panelGameObject = Instantiate(GameManager.Instance.Settings.uiPanelSettings.queueItemPrefab, panelParent);
            ActionQueuePanelItem queueItem = panelGameObject.GetComponent<ActionQueuePanelItem>();
            panelItems.Add(queueItem);
        }
    }

    private void Update()
    {
        if (panelItems.Count > 0)
        {
            panelItems[0].frontImage.fillAmount = panelItems[0].RemainingTime / panelItems[0].WaitDurationSeconds;
        }
    }

    //add thing to be updated in real time to keep the item fillamount up to date
}
