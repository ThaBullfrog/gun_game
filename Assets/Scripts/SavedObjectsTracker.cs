using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class SavedObjectsTracker : MonoBehaviour
{
    public static SavedObjectsTracker obj;
    public static List<GameObject> characters = new List<GameObject>();
    public static List<GameObject> checkpoints = new List<GameObject>();
    public static List<GameObject> all = new List<GameObject>();

    private void Awake()
    {
        ClearReferences();
        obj = this;
    }

    public static void AddCharacterReference(GameObject character)
    {
        characters.Add(character);
        all.Add(character);
    }

    public static void RemoveCharacterReference(GameObject character)
    {
        characters.Remove(character);
        all.Remove(character);
    }
    
    public static void AddCheckpointReference(GameObject checkpoint)
    {
        checkpoints.Add(checkpoint);
        all.Add(checkpoint);
    }

    public static void RemoveCheckpointReference(GameObject checkpoint)
    {
        checkpoints.Remove(checkpoint);
        all.Remove(checkpoint);
    }

    public static void ClearReferences()
    {
        characters.Clear();
        checkpoints.Clear();
        all.Clear();
    }

    public static void DestroyAllSavedObjects()
    {
        foreach(GameObject obj in all)
        {
            Destroy(obj);
        }
        ClearReferences();
    }
}