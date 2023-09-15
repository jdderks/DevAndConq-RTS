using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(Unit))]
public class UnitEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Unit unit = (Unit)target;
        EditorGUILayout.LabelField(unit.CurrentTask != null ? unit.CurrentTask.ToString() : "null");
        bool taskDebuginfo = unit.TaskDebugInfo;
        if (taskDebuginfo)
        {
            if (unit.CurrentTask is MoveUnitTask)
            {
                var moveUnitTask = unit.CurrentTask as MoveUnitTask;
                EditorGUILayout.LabelField(moveUnitTask.TaskState.ToString());
            }
        }
        DrawDefaultInspector();
    }
}
