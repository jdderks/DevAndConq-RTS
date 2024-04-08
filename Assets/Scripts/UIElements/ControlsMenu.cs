using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlsMenu : MonoBehaviour
{
    [SerializeField] private KeyCode controlsMenuPrefab;

    [SerializeField] private GameObject controlsMenu;

    private void Update()
    {
        if (Input.GetKeyDown(controlsMenuPrefab))
        {
            ToggleControlsMenu();
        }
    }

    private void ToggleControlsMenu()
    {
        controlsMenu.SetActive(!controlsMenu.activeSelf);
    }

}
