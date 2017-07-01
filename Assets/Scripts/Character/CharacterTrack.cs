using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class CharacterTrack : MonoBehaviour
{
    private void Start()
    {
        CharacterTracker.AddCharacterReference(gameObject);
    }

    private void OnDestroy()
    {
        CharacterTracker.RemoveCharacterReference(gameObject);
    }
}