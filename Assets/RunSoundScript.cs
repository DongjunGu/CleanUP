using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunSoundScript : MonoBehaviour
{
    public AudioSource runAudio;
    public AudioClip clipRun;
    public void RunSound()
    {
        SoundController.Instance.PlaySoundRun("Run", clipRun, 0.2f);
    }
}
