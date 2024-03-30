using TMPro;
using UnityEngine;
using NaughtyAttributes;
using UnityEngine.UI;
using System.Collections.Generic;

public class ValueManager : MonoBehaviour
{
    public string path = "Assets/Testing/Values.json";

    [SerializeField] private ValueHolder valueHolder;
    private NoiseValues originalValues;

    private Stack<NoiseValues> undoStack = new Stack<NoiseValues>();
    private Stack<NoiseValues> redoStack = new Stack<NoiseValues>();

    public TMP_InputField inResolution;
    public TMP_InputField inNoiseScale;
    public TMP_InputField inSeed;
    public TMP_InputField inOctaves;

    [SerializeField] private InputManagerBrechje inputManager;
    private UndoRedoManager undoRedoManager = new UndoRedoManager();


    [SerializeField]
    private Image img;

    public ValueHolder ValueHolder
    {
        get => valueHolder;
        set
        {
            OnValuesChanged();
            valueHolder = value;
        }
    }

    private void OnValuesChanged()
    {
        inResolution.text = ValueHolder.Values.resolution.ToString();
        //inNoiseScale.text = ValueHolder.Values.noiseScale.ToString();
        //inSeed.text = ValueHolder.Values.seed.ToString();
        // Update other UI elements as needed
    }

    private void Start()
    {

        inputManager.undoAction += Undo;
        inputManager.redoAction += Redo;

        // Save the original state for undo/redo
        originalValues = ValueHolder.Values;
    }

    [Button("SET TEXTURE")]
    public void SetImage()
    {
        ValueHolder.Texture = NoiseGenerator.GenerateNoiseTexture(ValueHolder.Values);
        ValueHolder.Texture.Apply();
        img.sprite = Sprite.Create(ValueHolder.Texture, new Rect(0, 0, ValueHolder.Texture.width, ValueHolder.Texture.height), new Vector2(0.5f, 0.5f), 100);
    }


    public void SetResolution()
    {
        StorePreviousState();

        //change current state
        string a = inResolution.text;
        ValueHolder.Values.resolution = int.Parse(a);

        //update the view
        SetImage();

        redoStack.Clear();
    }

    #region setters

    public void SetNoiseScale()
    {
        StorePreviousState();
        var b = inNoiseScale.text;
        ValueHolder.Values.noiseScale = float.Parse(b);
    }

    public void SetSeed()
    {
        StorePreviousState();
        var c = inSeed.text;
        ValueHolder.Values.seed = int.Parse(c);
    }

    public void SetOctaves()
    {
        StorePreviousState();
        var d = inOctaves.text;
        ValueHolder.Values.octaves = int.Parse(d);
    }
    #endregion

    private void StorePreviousState()
    {
        undoRedoManager.StorePreviousState(ValueHolder.Values);
    }

    public void Undo()
    {
        NoiseValues previousValues = undoRedoManager.Undo(ValueHolder.Values);
        ApplyValues(previousValues);
    }

    public void Redo()
    {
        NoiseValues nextValues = undoRedoManager.Redo(ValueHolder.Values);
        ApplyValues(nextValues);
    }


    private void ApplyValues(NoiseValues updated)
    {
        ValueHolder.Values = updated;
        SetImage();
        DataChanged();
    }


    public void DataChanged()
    {
        if (ValueHolder.Values.resolution.ToString() != inResolution.text)
        {
            inResolution.text = ValueHolder.Values.resolution.ToString();
        }
        //if (ValueHolder.Values.noiseScale.ToString() != inNoiseScale.text)
        //{
        //    inNoiseScale.text = ValueHolder.Values.noiseScale.ToString();
        //}
        //if (ValueHolder.Values.seed.ToString() != inSeed.text)
        //{
        //    inSeed.text = ValueHolder.Values.seed.ToString();
        //}
        //if (ValueHolder.Values.octaves.ToString() != inOctaves.text)
        //{
        //    inOctaves.text = ValueHolder.Values.octaves.ToString();
        //}
        //if (ValueHolder.Values.persistance.ToString() != inPersistance.text)
        //{
        //    inPersistance.text = ValueHolder.Values.persistance.ToString();
        //}
        //if (ValueHolder.Values.lacunarity.ToString() != inLacunarity.text)
        //{
        //    inLacunarity.text = ValueHolder.Values.lacunarity.ToString();
        //}
        //if (ValueHolder.Values.offset.x.ToString() != inOffsetX.text)
        //{
        //    inOffsetX.text = ValueHolder.Values.offset.x.ToString();
        //}
        //if (ValueHolder.Values.offset.y.ToString() != inOffsetY.text)
        //{
        //    inOffsetY.text = ValueHolder.Values.offset.y.ToString();
        //}
    }

}