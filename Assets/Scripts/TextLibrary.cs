using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using UnityEngine;

public enum LanguageSets
{
    English = 0,
    Dutch = 1
}

public class TextLibrary
{

    public static InfoPanelDescriptorText LoadInfoPanelTextFromJson(string path)
    {
        string json = System.IO.File.ReadAllText(path);
        InfoPanelDescriptorText info = JsonUtility.FromJson<InfoPanelDescriptorText>(json);
        return info;
    }


    public static InfoPanelDescriptorText LoadInfoPanelText(string path)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(InfoPanelDescriptorText));
        FileStream stream = new FileStream(path, FileMode.Open);

        InfoPanelDescriptorText info = (InfoPanelDescriptorText)serializer.Deserialize(stream);
        
        stream.Close();

        return info;
    }
}

[System.Serializable]
public class PanelInformation
{
    public int panelID;
}

[System.Serializable]
public class InfoPanelDescriptorText : PanelInformation
{
    public string InfoPanelHeader;
    public string AmountOfSelectedUnits;
}
