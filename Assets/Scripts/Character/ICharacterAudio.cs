using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public interface ICharacterAudio
{
    void PlayJump();
    void PlayFootstep();
    void PlayGunshot();
    void PlayDeath();
    void PlayGrappleConnect();
}