using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class SoundController : MonoBehaviour
{
    public static SoundController Instance;
    public AudioSource BackGroungMusic;
    public List<AudioClip> bgmClips;
    public AudioMixerGroup playerAudioMixerGroup;
    public AudioMixerGroup playerRunMixerGroup;
    public AudioMixerGroup robotAudioMixerGroup;
    public AudioMixerGroup deskAudioMixerGroup;
    public AudioMixerGroup mouseAudioMixerGroup;
    public AudioMixerGroup bossAudioMixerGroup;
    public static int bgmNum = 0;
    int currentBgmNumber;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        int currentBgmNumber = bgmNum;
        PlayBackgroundMusic();

    }
    void Update()
    {
        
        if (bgmNum != currentBgmNumber)
        {
            currentBgmNumber = bgmNum;
            PlayBackgroundMusic();
        }
    }
    

    public void PlaySound(string name, AudioClip clip)
    {
        GameObject go = new GameObject(name + "Sound");
        AudioSource audioSource = go.AddComponent<AudioSource>();
        audioSource.outputAudioMixerGroup = playerAudioMixerGroup;
        audioSource.clip = clip;
        audioSource.Play();
        Destroy(go, clip.length);
    }
    public void PlayObjectSoundRobot(string name, AudioClip clip)
    {
        GameObject go = new GameObject(name + "Sound");
        AudioSource audioSource = go.AddComponent<AudioSource>();
        audioSource.outputAudioMixerGroup = robotAudioMixerGroup;
        audioSource.clip = clip;
        audioSource.Play();
        Destroy(go, clip.length);
    }
    public void PlayLaserRobot(string name, AudioClip clip)
    {
        GameObject go = new GameObject(name + "Sound");
        AudioSource audioSource = go.AddComponent<AudioSource>();
        audioSource.outputAudioMixerGroup = robotAudioMixerGroup;
        audioSource.spatialBlend = 0.8f;
        audioSource.clip = clip;
        audioSource.Play();
        Destroy(go, clip.length);
    }
    public void PlaySoundLoopRobot(string name, AudioClip clip, float dur)
    {
        GameObject go = new GameObject(name + "Sound");
        AudioSource audioSource = go.AddComponent<AudioSource>();
        audioSource.outputAudioMixerGroup = robotAudioMixerGroup;
        audioSource.clip = clip;
        audioSource.loop = true;
        audioSource.Play();
        Destroy(go, dur);
    }
    public void PlaySoundDesk(string name, AudioClip clip)
    {
        GameObject go = new GameObject(name + "Sound");
        AudioSource audioSource = go.AddComponent<AudioSource>();
        audioSource.outputAudioMixerGroup = deskAudioMixerGroup;
        audioSource.clip = clip;
        audioSource.Play();
        Destroy(go, clip.length);
    }
    public void PlaySoundMouse(string name, AudioClip clip, float dur)
    {
        GameObject go = new GameObject(name + "Sound");
        AudioSource audioSource = go.AddComponent<AudioSource>();
        audioSource.outputAudioMixerGroup = mouseAudioMixerGroup;
        audioSource.clip = clip;
        audioSource.spatialBlend = 0.8f;
        audioSource.Play();
        Destroy(go, dur);
    }
    public void PlayLoop(string name, AudioClip clip)
    {
        GameObject go = new GameObject(name + "Sound");
        AudioSource audioSource = go.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.loop = true;
        audioSource.Play();
    }

    public void PlayType(string name, AudioClip clip , float dur)
    {
        GameObject go = new GameObject(name + "Sound");
        AudioSource audioSource = go.AddComponent<AudioSource>();
        audioSource.outputAudioMixerGroup = playerAudioMixerGroup;
        audioSource.clip = clip;
        audioSource.Play();
        Destroy(go, dur);
    }
    public void PlaySoundRun(string name, AudioClip clip, float dur)
    {
        GameObject go = new GameObject(name + "Sound");
        AudioSource audioSource = go.AddComponent<AudioSource>();
        audioSource.outputAudioMixerGroup = playerRunMixerGroup;
        audioSource.clip = clip;
        audioSource.Play();
        
        Destroy(go, dur);
    }
    public void PlaySoundWalk(string name, AudioClip clip)
    {
        GameObject go = new GameObject(name + "Sound");
        AudioSource audioSource = go.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.Play();

        Destroy(go, 0.1f);
    }

    public void PlaySoundOneShot(string name, AudioClip clip)
    {
        GameObject go = new GameObject(name + "Sound");
        AudioSource audioSource = go.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.PlayOneShot(clip);
        Destroy(go,2f);
    }

    private void PlayBackgroundMusic()
    {
        if (currentBgmNumber >= 0 && currentBgmNumber < bgmClips.Count)
        {
            if(currentBgmNumber == 0 || currentBgmNumber == 2)
            {
                BackGroungMusic.volume = 0.06f;
            }
            else if(currentBgmNumber == 5)
            {
                BackGroungMusic.volume = 0.1f;
            }
            else
            {
                BackGroungMusic.volume = 0.2f;
            }
            BackGroungMusic.clip = bgmClips[currentBgmNumber];
            BackGroungMusic.loop = true;
            BackGroungMusic.Play();
        }
    }

    public void PlayBossSound(string name, AudioClip clip, float dur)
    {
        GameObject go = new GameObject(name + "Sound");
        AudioSource audioSource = go.AddComponent<AudioSource>();
        audioSource.outputAudioMixerGroup = bossAudioMixerGroup;
        audioSource.clip = clip;
        audioSource.Play();
        Destroy(go, clip.length);
    }

    public void PlayBossSoundSpat(string name, AudioClip clip, float dur)
    {
        GameObject go = new GameObject(name + "Sound");
        AudioSource audioSource = go.AddComponent<AudioSource>();
        audioSource.outputAudioMixerGroup = bossAudioMixerGroup;
        audioSource.spatialBlend = 0.8f;
        audioSource.clip = clip;
        audioSource.Play();
        Destroy(go, clip.length);
    }
    public void MuteBackgroundMusic()
    {
        BackGroungMusic.mute = true;
    }
    public void ResumeBackgroundMusic()
    {
        BackGroungMusic.mute = false;
    }
}
