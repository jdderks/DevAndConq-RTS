using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PanelItem : MonoBehaviour
{
    public Image image;
    public TextMeshProUGUI itemText;
    public TextMeshProUGUI textCost;
    public Button button;

    public void SetPanelItemInfo(Image image, string buttonText, string textCost)
    {
        this.image = image;
        this.textCost.text = textCost;
        itemText.text = buttonText;
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
