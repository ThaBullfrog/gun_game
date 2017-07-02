using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class CharacterAudio : MonoBehaviour, ICharacterAudio
{
    public AudioSource gunshot;
    public AudioSource jump;
    public AudioSource footsteps;
    public AudioSource grappleConnect;
    public AudioClip[] footstepClips;
    public AudioClip death;

    private int footstepIndex = 0;

    private void Start()
    {
        IDeathAlert deathAlert = GetComponent<IDeathAlert>();
        if (deathAlert != null)
        {
            deathAlert.onDeath += PlayDeath;
        }
    }

    public void PlayJump()
    {
        jump.Play();
    }

    public void PlayFootstep()
    {
        if (footstepClips == null)
        {

        }
        if (footstepClips.Length > 0)
        {
            footsteps.clip = footstepClips[footstepIndex];
            footsteps.Play();
            footstepIndex += 1;
            if (footstepIndex >= footstepClips.Length)
            {
                footstepIndex = 0;
            }
        }
    }

    public void PlayGunshot()
    {
        gunshot.Play();
    }

    public void PlayDeath()
    {
        PlaySound.PlayClipAtPosition(death, transform.position);
    }

    public void PlayGrappleConnect()
    {
        grappleConnect.Play();
    }
}