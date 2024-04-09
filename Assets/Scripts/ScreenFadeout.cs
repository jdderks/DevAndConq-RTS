using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFadeout : MonoBehaviour
{
    [SerializeField] private Image imageToFade;
    [SerializeField] private float fadeDuration = 1f;

    public void FadeToBlack()
    {
        StartCoroutine(FadeImageToBlack());
    }

    IEnumerator FadeImageToBlack()
    {
        while(imageToFade.color.a < 1)
        {
            imageToFade.color = new Color(imageToFade.color.r, imageToFade.color.g, imageToFade.color.b, imageToFade.color.a + (Time.deltaTime / fadeDuration));
            yield return null;
        }
    }
}
