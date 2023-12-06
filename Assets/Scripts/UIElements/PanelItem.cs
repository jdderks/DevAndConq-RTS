using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PanelItem : MonoBehaviour
{
    public Image image;
    public TextMeshProUGUI itemText; 

    public void SetPanelItemInfo(Image image, string text)
    {
        this.image = image;
        itemText.text = text;
    }
}
