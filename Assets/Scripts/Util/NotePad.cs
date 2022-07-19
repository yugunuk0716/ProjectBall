using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class NotePad : EditorWindow
{
    private string myString;

    [MenuItem("Window/NotePad")]
    public static void ShowWindow()
    {
        GetWindow(typeof(NotePad));
    }

    void OnGUI()
    {
        GUILayout.Label("", EditorStyles.boldLabel);
        GUILayout.Label("준형이 전용 메모장", EditorStyles.boldLabel);
        GUILayout.Label("", EditorStyles.boldLabel);
        GUILayout.Label(PlayerPrefs.GetString("data"), EditorStyles.boldLabel);
        GUILayout.Label("", EditorStyles.boldLabel);
        myString = EditorGUILayout.TextField(myString, GUILayout.Width(500));
    }

    private void OnLostFocus()
    {
        PlayerPrefs.SetString("data", myString);
    }
}
