using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionQueuePanel : MonoBehaviour
{
    [SerializeField] private Transform panelParent;
    public List<QueuePanelItem> panelItems = new();

    private void UpdateActionQueuePanel(List<ActionQueueItem> queueItems)
    {
        foreach (var panel in panelItems)
        {
            //maybe remove click events later as well
            Destroy(panel);
        }
        panelItems = new();

        foreach (var item in queueItems)
        {
             var panelGameObject = Instantiate(GameManager.Instance.Settings.uiPanelSettings.queueItemPrefab, panelParent);
            
            

        }

        //Remove all existing if there are any

        //Get action queue

        //For each actionqueue item: instantiate UIitem

        
    }

    //add thing to be updated in real time to keep the item fillamount up to date
}
