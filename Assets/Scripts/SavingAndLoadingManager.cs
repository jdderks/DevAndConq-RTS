using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class SavingAndLoadingManager : MonoBehaviour
{
    public void SaveValues(ValueSetter setter)
    {
        var holder = setter.ValueHolder;

        string json = JsonUtility.ToJson(holder.Values);
        File.WriteAllText(setter.path, json);
    }

    public void LoadValues(ValueSetter setter)
    {
        var holder = setter.ValueHolder;

        string json = File.ReadAllText(setter.path);
        holder.Values = JsonUtility.FromJson<NoiseValues>(json);
    }

    public void ExportImage(ValueSetter setter)
    {
        var holder = setter.ValueHolder;

        byte[] bytes = holder.Texture.EncodeToPNG();
        File.WriteAllBytes(setter.path, bytes);
    }
}