using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class CharacterTracker : MonoBehaviour
{
    public static CharacterTracker obj;
    public static List<GameObject> characters { get { return obj._characters; } }

    public List<GameObject> _characters = new List<GameObject>();

    private void Awake()
    {
        obj = this;
    }

    public static void AddCharacterReference(GameObject character)
    {
        characters.Add(character);
    }

    public static void RemoveCharacterReference(GameObject character)
    {
        characters.Remove(character);
    }

    public static void ClearReferences()
    {
        characters.Clear();
    }

    public static void DestroyAllCharacters()
    {
        foreach(GameObject obj in characters)
        {
            Destroy(obj);
        }
        ClearReferences();
    }
}