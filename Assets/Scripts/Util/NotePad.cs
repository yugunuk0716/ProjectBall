using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
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
        GUILayout.Label("lostArk only", EditorStyles.boldLabel);
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
#endif