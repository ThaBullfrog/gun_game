using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class CharacterTrack : MonoBehaviour
{
    private void Start()
    {
        SavedObjectsTracker.AddCharacterReference(gameObject);
    }

    private void OnDestroy()
    {
        SavedObjectsTracker.RemoveCharacterReference(gameObject);
    }
}