using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MasterVolScript : MonoBehaviour
{
    public AudioMixer MasterMixer;
    public static MasterVolScript _instance;
    public static MasterVolScript Instance { get { return _instance; } }
    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        _instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
    public void ControlAudio(float soundLevel)
    {
        MasterMixer.SetFloat("MasterVol", Mathf.Log10(soundLevel) * 20);
    }
}
