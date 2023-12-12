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
            GameObject panelGameObject = Instantiate(GameManager.Instance.Settings.uiPanelSettings.queueItemPrefab, panelParent);
            ActionQueuePanelItem queueItem = panelGameObject.GetComponent<ActionQueuePanelItem>();
            panelItems.Add(queueItem);
        }
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

    public void UpdateActionQueuePanel()
    {
        if (!panelIsActive) return; // Early return if the panel is not active

        if (focussedActionQueue.Items.Count > 0)
        {
            var progress = focussedActionQueue.Items[0].RemainingTime / focussedActionQueue.Items[0].WaitForSeconds;

            // Ensure the progress is clamped between 0 and 1
            var clampedProgress = Mathf.Clamp01(progress);

            // Invert the progress to fill from 0 to 1
            var invertedProgress = 1f - clampedProgress;

            Debug.Log(invertedProgress);
            if (panelItems.Count > 0)
            {
                panelItems[0].frontImage.fillAmount = invertedProgress;
            }
        }
    }


    private void Update()
    {
        UpdateActionQueuePanel();
    }

    //add thing to be updated in real time to keep the item fillamount up to date
}
