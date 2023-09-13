using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MainGameSettings))]
public class GameSettingsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        //MainGameSettings mainObject = (MainGameSettings)target;

        //EditorGUI.BeginChangeCheck();
        //mainObject.modelSettings = (ModelSettings)EditorGUILayout.ObjectField("Sub Object", mainObject.modelSettings, typeof(ModelSettings), false);

        //if (EditorGUI.EndChangeCheck())
        //{
        //    // If the selection changes, mark the object as dirty to save changes.
        //    EditorUtility.SetDirty(mainObject);
        //}

        DrawDefaultInspector();
    }
}