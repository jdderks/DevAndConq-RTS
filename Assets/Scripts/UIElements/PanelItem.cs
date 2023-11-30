using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PanelItem : MonoBehaviour
{
    public Sprite sprite;
    public TextMeshProUGUI itemText; 

    public void SetPanelItemInfo(Sprite sprite, string text)
    {
        this.sprite = sprite;
        itemText.text = text;
    }
}
