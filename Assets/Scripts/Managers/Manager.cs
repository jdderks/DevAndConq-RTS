using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    [SerializeField] private bool debugMode = false;

    public bool DebugMode { get => debugMode; set => debugMode = value; }
}
