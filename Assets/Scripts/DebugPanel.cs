//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using System.Reflection;
//using Unity.VisualScripting;
//using UnityEngine;

//public class DebugPanel : MonoBehaviour
//{
//    private void OnGUI()
//    {
//        Rect labelRect = new Rect(10, 10, 200, 30);

//        GUIStyle style = new GUIStyle(GUI.skin.label);
//        style.fontSize = 24;
//        style.normal.textColor = Color.white;

//        var attributes = UpdateAllAttributes();

//        for (int i = 0; i < attributes.Count; i++)
//        {
//            int margin = 2;
//            labelRect.y += (style.fontSize + margin) * i;
//            GUI.Label(labelRect, attributes[i], style);
//        }

//    }

//    private List<string> UpdateAllAttributes()
//    {
//        Type type = GetType();
//        List<string> strings = new List<string>();

//        // Get fields with the custom attribute
//        FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

//        foreach (FieldInfo field in fields)
//        {
//            DebugPanelAttribute[] debugAttributes = (DebugPanelAttribute[])Attribute.GetCustomAttributes(field, typeof(DebugPanelAttribute));
//            if (debugAttributes.Length > 0)
//            {
//                foreach (DebugPanelAttribute attribute in debugAttributes)
//                {
//                    strings.Add(attribute.Description + " " + attribute.ToString());
//                }
//            }
//        }

//        // Get properties with the custom attribute
//        PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

//        foreach (PropertyInfo property in properties)
//        {
//            DebugPanelAttribute[] debugAttributes = (DebugPanelAttribute[])Attribute.GetCustomAttributes(property, typeof(DebugPanelAttribute));
//            if (debugAttributes.Length > 0)
//            {
//                foreach (DebugPanelAttribute attribute in debugAttributes)
//                {
//                    strings.Add(attribute.Description + " " + attribute.ToString());
//                }
//            }
//        }

//        return strings;
//    }

//}
