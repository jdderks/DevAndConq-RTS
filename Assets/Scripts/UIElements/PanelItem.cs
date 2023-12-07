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
    public Button button;

    public void SetPanelItemInfo(Image image, string text, UnityAction onClickListener = null)
    {
        this.image = image;
        itemText.text = text;
        button.onClick.AddListener(onClickListener);
    }

    public void AddEvents()
    {
        Debug.Log("Button pressed");
    }

    public void RemoveEvents()
    {
        button.onClick.RemoveAllListeners();
    }
}
