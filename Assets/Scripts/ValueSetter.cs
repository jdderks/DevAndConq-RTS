using TMPro;
using UnityEngine;
using NaughtyAttributes;
using UnityEngine.UI;

//public enum ValueType
//{
//    Resolution = 0,
//    NoiseScale = 1,
//    Seed = 2,
//    Octaves = 3,
//    Persistance = 4,
//    Lacunarity = 5,
//    Offset = 6
//}

public class ValueSetter : MonoBehaviour
{
    [SerializeField] private ValueHolder valueHolder;

    public string path = "Assets/Testing/Values.json";

    public TMP_InputField inResolution;
    public TMP_InputField inNoiseScale;
    public TMP_InputField inSeed;
    public TMP_InputField inOctaves;
    //public TMP_InputField inPersistance;
    //public TMP_InputField inLacunarity;
    //public TMP_InputField inOffset;

    private Image img;

    public ValueHolder ValueHolder { get => valueHolder; set => valueHolder = value; }

    private void OnValuesChanged()
    {
        inResolution.text = ValueHolder.Values.resolution.ToString();
        inNoiseScale.text = ValueHolder.Values.noiseScale.ToString();
        inSeed.text = ValueHolder.Values.seed.ToString();
        //inPersistance.text = values.persistance.ToString();
        //inLacunarity.text = values.lacunarity.ToString();
        //inOffset.text = values.offset.ToString();
    }

    private void OnValidate()
    {
        OnValuesChanged();
    }

    #region setters
    [Button("SET TEXTURE")]
    public void SetImage()
    {
        ValueHolder.Texture = NoiseGenerator.GenerateNoiseTexture(ValueHolder.Values);
        ValueHolder.Texture.Apply();
        img.sprite = Sprite.Create(ValueHolder.Texture, new Rect(0, 0, ValueHolder.Texture.width, ValueHolder.Texture.height), new Vector2(0.5f, 0.5f), 100);
    }

    public void SetResolution()
    {
        var a = inResolution.text;
        ValueHolder.Values.resolution = int.Parse(a);
    }

    public void SetNoiseScale()
    {
        var b = inNoiseScale.text;
        ValueHolder.Values.noiseScale = float.Parse(b);
    }

    public void SetSeed()
    {
        var c = inSeed.text;
        ValueHolder.Values.seed = int.Parse(c);
    }

    public void SetOctaves()
    {
        var d = inOctaves.text;
        ValueHolder.Values.octaves = int.Parse(d);
    }
    #endregion
}
