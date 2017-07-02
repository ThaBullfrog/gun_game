using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    private bool _paused;
    private bool paused
    {
        get { return _paused; }
        set
        {
            _paused = value;
            if(value)
            {
                Time.timeScale = 0f;
            }
            else
            {
                Time.timeScale = 1f;
            }
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            paused = !paused;
        }
    }

    private void OnGUI()
    {
        if(paused)
        {
            GUIUtils.DrawWhiteBackground();

            GUIUtils.BeginCentered();
            if(GUILayout.Button("Restart from checkpoint", GUIUtils.buttonStyle))
            {
                paused = false;
                Game.RestartLevel();
            }
            if(GUILayout.Button("Restart from beginning of level", GUIUtils.buttonStyle))
            {
                Game.DeleteSave();
                paused = false;
                Game.RestartLevel();
            }
            GUIUtils.EndCentered();
        }
    }
}