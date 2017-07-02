using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public static class GUIUtils
{
    public static void BeginCentered()
    {
        GUILayout.BeginArea(new Rect(Vector2.zero, new Vector2(Screen.width, Screen.height)));
        GUILayout.FlexibleSpace();
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.BeginVertical();
    }

    public static void EndCentered()
    {
        GUILayout.EndVertical();
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.EndArea();
    }

    public static GUIStyle bgStyle;
    public static GUIStyle labelStyle;
    public static GUIStyle buttonStyle;

    public static void SetupGUIStyles()
    {
        bgStyle = new GUIStyle();
        labelStyle = new GUIStyle();
        buttonStyle = new GUIStyle(GUI.skin.button);
        bgStyle.normal.background = Utils.ColorTexture(Color.white);
        labelStyle.normal.textColor = Color.black;
        labelStyle.fontSize = 36;
        buttonStyle.fontSize = 36;
    }

    public static void DrawWhiteBackground()
    {
        GUI.Box(new Rect(Vector2.zero, new Vector2(Screen.width, Screen.height)), "", GUIUtils.bgStyle);
    }
}