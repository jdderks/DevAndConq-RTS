using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private InfoPanel infoPanel;


    [SerializeField] private LanguageSets currentSelectedLanguage = LanguageSets.English;

    private TextMeshProUGUI InfoPanelText_UnitAmount;


    public void UpdateInfoPanelValues()
    {
        InfoPanelText_UnitAmount.text = GameManager.Instance.unitManager.Units.Count.ToString();
    }

    public void SetInfoPanelDescriptors()
    {
        InfoPanelDescriptorText a = TextLibrary.LoadInfoPanelTextFromJson("Xml/" + (int)currentSelectedLanguage + "_InfoPanelText.json");
        
        infoPanel.InfoPanelHeaderDescriptor.text = a.InfoPanelHeader;
        infoPanel.AmountOfUnitsSelectedDescriptor.text = a.AmountOfSelectedUnits;
    }

    [Button("Update language")]
    public void SetLanguage()
    {
        SetInfoPanelDescriptors();
    }
}
