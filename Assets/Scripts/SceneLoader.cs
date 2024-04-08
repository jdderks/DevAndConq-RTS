using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    public void LoadScene(string scene)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(scene);
    }



    public void LoadSceneWithDelay(string scene)
    {
        StartCoroutine(StartWithDelay(scene));    
    }


    public IEnumerator StartWithDelay(string scene)
    {
        yield return new WaitForSeconds(1);
        LoadScene(scene);
    }
}
