using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    [SerializeField] private InfoPanel infoPanel;
    [SerializeField] private RtsActionPanel rtsActionPanel;
    [SerializeField] private ActionQueuePanel actionQueuePanel;

    [SerializeField] private LanguageSets currentSelectedLanguage = LanguageSets.English;

    //private TextMeshProUGUI InfoPanelText_UnitAmount;

    public void UpdateRtsActionPanel(List<Unit> units = null, Building building = null)
    {
        var actions = new List<RtsAction>();

        if (building != null)
        {
            actions.AddRange(building.rtsBuildingActions);
        }

        if (units != null && units.Count > 0)
        {
            if (building != null)
            {
                actions = actions.Intersect(units[0].RtsActions).ToList();
            }
            else
            {
                actions.AddRange(units[0].RtsActions);
            }

            for (int i = 1; i < units.Count; i++)
            {
                actions = actions.Intersect(units[i].RtsActions).ToList();
            }
        }

        rtsActionPanel.UpdatePanels(actions);
    }

    public void UpdateActionQueuePanel(ActionQueue queue)
    {
        if (queue == null) //Just make the queue UI empty
        {

        }
        else //Fill the queue UI with corresponding icons and things
        {
            List<ActionQueueItem> queueItems = queue.Actions;
        }
    }


    public void UpdateInfoPanelValues()
    {
        infoPanel.AmountOfUnitsSelectedValue.text = GameManager.Instance.SelectableCollection.selectedTable.Values.Count.ToString();
    }

    private void SetInfoPanelDescriptors()
    {
        InfoPanelDescriptorText a =
            TextLibrary.LoadInfoPanelTextFromJson("Xml/" + (int)currentSelectedLanguage + "_InfoPanelText.json");

        infoPanel.InfoPanelHeaderDescriptor.text = a.InfoPanelHeader;
        infoPanel.AmountOfUnitsSelectedDescriptor.text = a.AmountOfSelectedUnits;
    }

    [Button("Update language")]
    public void SetLanguage()
    {
        SetInfoPanelDescriptors();
    }
}