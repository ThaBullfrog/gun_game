using UnityEditor;
using UnityEngine;
using System.Collections;

[CustomEditor(typeof(UpdatePrefabs))]
public class UpdatePrefabsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        UpdatePrefabs updator = target as UpdatePrefabs;
        if(updator != null)
        {
            if(GUILayout.Button("Update all Prefabs"))
            {
                updator.UpdateAllPrefabs();
            }
        }
    }
}