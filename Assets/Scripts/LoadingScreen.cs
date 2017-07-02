using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class LoadingScreen : MonoBehaviour
{
    private static LoadingScreen main;

    public bool active;
    private static bool startActive = false;

    private void Start()
    {
        main = this;
        active = startActive;
    }

    private void OnGUI()
    {
        if (active)
        {
            GUIUtils.DrawWhiteBackground();

            GUIUtils.BeginCentered();
            GUILayout.Label("Loading...", GUIUtils.labelStyle);
            GUIUtils.EndCentered();
        }
    }

    public static void Show()
    {
        if (main == null)
        {
            startActive = true;
        }
        else
        {
            main.active = true;
        }
    }

    public static void Hide()
    {
        main.active = false;
    }
}