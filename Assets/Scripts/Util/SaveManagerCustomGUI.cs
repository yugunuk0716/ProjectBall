using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SaveManager))]
public class SaveManagerCustomGUI : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        var script = (SaveManager) target;

        if(GUILayout.Button("Save Map"))
        {
            script.SaveMap();
        }

        if (GUILayout.Button("Load Map From Json"))
        {
            script.LoadMapJson();
        }

        if (GUILayout.Button("Load Map From Spreadsheets"))
        {
            Debug.Log("오류로 인한 막아둠");
            //script.LoadMapSpreadsheets();
        }
    }
}
