using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ActionPanelItem : MonoBehaviour
{
    public Image image;
    public TextMeshProUGUI itemText;
    public TextMeshProUGUI textCost;
    public Button button;
    public float actionDelay;

    public void SetPanelItemInfo(Image image, string buttonText, string textCost, float actionDelay)
    {
        this.image = image;
        this.textCost.text = textCost;
        itemText.text = buttonText;
        this.actionDelay = actionDelay;
    }

    public void SetButtonInteraction(UnityAction onClickListener)
    {
        button.onClick.AddListener(onClickListener);
    }

    public void RemoveEvents()
    {
        button.onClick.RemoveAllListeners();
    }
}
