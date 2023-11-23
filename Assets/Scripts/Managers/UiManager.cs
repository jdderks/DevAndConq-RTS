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


    [SerializeField] private LanguageSets currentSelectedLanguage = LanguageSets.English;

    //private TextMeshProUGUI InfoPanelText_UnitAmount;

    public void UpdateRtsActionPanel(List<Unit> units = null, Building building = null)
    {
        List<RtsAction> actions = new();

        actions.AddRange(UpdateRtsPanelUnits(units));
        actions.AddRange(UpdateRtsPanelBuilding(building));

        foreach (var action in actions)
        {
            
        }

        //
        // actions.Add(UpdateRtsPanelBuilding(building));
    }

    private List<RtsUnitAction> UpdateRtsPanelUnits(List<Unit> units = null)
    {
        if (units == null) return null;
        if (units.count == 0) return null;

        List<RtsUnitAction> actions = new();
        foreach (Unit unit in units)
        {
            foreach (var action in unit.RtsActions)
            {
                if (actions.Contains(action)) continue;
                actions.Add(action);
            }
        }

        return actions;
    }

    private List<RtsBuildingAction> UpdateRtsPanelBuilding(Building building)
    {
        if (building == null) return null;
        return building.buildingActions.Count != 0 ? building.buildingActions : null;
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