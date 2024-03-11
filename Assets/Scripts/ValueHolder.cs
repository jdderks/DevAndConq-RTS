using NaughtyAttributes;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ValueHolder : MonoBehaviour
{

    private Texture2D texture;

    [SerializeField] private NoiseValues values;

    public NoiseValues Values
    {
        get => values;
        set
        {
            values = value;
        }
    }

    public Texture2D Texture { get => texture; set => texture = value; }

}