using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MasterVolScript : MonoBehaviour
{
    public AudioMixer MasterMixer;
   
    public void ControlAudio(float soundLevel)
    {
        MasterMixer.SetFloat("MasterVol", Mathf.Log10(soundLevel) * 20);
    }
}
