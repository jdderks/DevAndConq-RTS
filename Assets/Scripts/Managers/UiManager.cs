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


    [SerializeField] private LanguageSets currentSelectedLanguage = LanguageSets.English;

    //private TextMeshProUGUI InfoPanelText_UnitAmount;

    public void UpdateRtsActionPanel(List<Unit> units = default, Building building = null)
    {
        var actions = new List<RtsAction>();

        // Check if units is not null and contains at least one unit.
        if (units != null && units.Count > 0)
        {
            // Get all the RtsActions of the first unit.
            actions.AddRange(units[0].RtsActions);

            // Loop through the remaining units and filter actions that are shared among all units.
            for (var i = 1; i < units.Count; i++)
            {
                actions = actions.Intersect(units[i].RtsActions).ToList();
            }
        }

        rtsActionPanel.UpdatePanels(actions);
    }





    public void UpdateInfoPanelValues()
    {
        infoPanel.AmountOfUnitsSelectedValue.text =
            GameManager.Instance.SelectableCollection.selectedTable.Values.Count.ToString();
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