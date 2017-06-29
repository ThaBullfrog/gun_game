using System.Collections.Generic;
using System.Collections;
using UnityEditor;
using UnityEngine;

public class LayerMaskToInteger : EditorWindow
{
    [MenuItem("Window/Layer Mask to Integer")]
    public static void ShowWindow()
    {
        GetWindow<LayerMaskToInteger>();
    }

    private LayerMask layerMask;

    void OnEnable()
    {
        layerMask = 0;
    }

    void OnGUI()
    {
        NewLine();
        layerMask = EditorTools.LayerMaskField("", layerMask);
        NewLine();
        GUILayout.Label("In integer form: " + ((int)layerMask).ToString());
        NewLine();
        if (GUILayout.Button("Copy to Clipboard"))
        {
            EditorGUIUtility.systemCopyBuffer = ((int)layerMask).ToString();
        }
    }

    private void NewLine()
    {
        GUILayout.Label("");
    }
}