using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class PlaySound : MonoBehaviour
{
    public AudioClip clip;

    private AudioSource audioSource;

    public static void PlayClipAtPosition(AudioClip clip, Vector3 position)
    {
        GameObject obj = new GameObject("play_sound");
        obj.transform.parent = Game.clones;
        obj.transform.position = position;
        PlaySound component = obj.AddComponent<PlaySound>();
        component.clip = clip;
    }

    private void Start()
    {
        audioSource = gameObject.AddAudioSource();
        audioSource.clip = clip;
        audioSource.Play();
    }

    private void Update()
    {
        if(!audioSource.isPlaying)
        {
            Destroy(gameObject);
        }
    }
}