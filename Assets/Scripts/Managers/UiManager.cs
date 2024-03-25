using NaughtyAttributes;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UiManager : Manager
{
    [SerializeField] private InfoPanel infoPanel;
    [SerializeField] private RtsActionPanel rtsActionPanel;
    [SerializeField] private ActionQueuePanel actionQueuePanel;
    [SerializeField] private GameObject actionQueuePanelGameObject;

    [SerializeField] private TextMeshProUGUI moneyAmountText;

    [SerializeField] private LanguageSets currentSelectedLanguage = LanguageSets.English;

    public ActionQueuePanel ActionQueuePanel { get => actionQueuePanel; set => actionQueuePanel = value; }
    public TextMeshProUGUI MoneyAmountText { get => moneyAmountText; set => moneyAmountText = value; }


    private void Update()
    {
        
    }

    public void UpdateEconomyUI()
    {
        var currentEconomy = GameManager.Instance.economyManager.GetEconomy(GameManager.Instance.teamManager.TeamCurrentlyControlling.teamByColour);
        moneyAmountText.text = currentEconomy.CurrentAmountOfMoney.ToString();
    }

    public void UpdateRtsActionPanel(ISelectable selectable)
    {
        var actions = new List<RtsAction>();

        if (selectable != null)
        {
            actions.AddRange(selectable.GetActions());
        }

        rtsActionPanel.UpdatePanels(actions);
    }


    public void OpenActionQueuePanel(ActionQueue queue)
    {
        actionQueuePanelGameObject.SetActive(true);
        ActionQueuePanel.TotalPanelReset(queue: queue);
    }

    public void CloseActionQueuePanel()
    {
        ActionQueuePanel.CloseActionQueueItems();
        actionQueuePanelGameObject.SetActive(false);
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