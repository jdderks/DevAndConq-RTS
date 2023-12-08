using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionQueueItem : MonoBehaviour
{
    public RtsAction actionToExecute;

    public Image backImage; //Often black & white / monochrome
    public Image frontImage; //Foreground image that is filled over time

    public float timeToFillSeconds = 5f;

    public void SetPanelItemInfo(Image backImage, Image frontImage, float timeToFillSeconds = 5f)
    {
        this.backImage = backImage;
        this.frontImage = frontImage;

        this.timeToFillSeconds = timeToFillSeconds;
    }
}
