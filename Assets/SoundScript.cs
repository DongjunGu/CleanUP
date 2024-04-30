using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundScript : MonoBehaviour
{
    public AudioClip clip1;
    public AudioClip clip2;
    public AudioClip clip3;
    public AudioClip clip4;
    public AudioClip clip5;
    AudioSource audioSource;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public void Sound1(AudioClip clip)
    {
        audioSource.clip = clip1;
        audioSource.Play();
    }
    public void Sound2(AudioClip clip)
    {
        audioSource.clip = clip2;
        audioSource.Play();
    }
    public void Sound3(AudioClip clip)
    {
        audioSource.clip = clip3;
        audioSource.Play();
    }
    public void Sound4(AudioClip clip)
    {
        audioSource.clip = clip4;
        audioSource.Play();
    }
    public void Sound5(AudioClip clip)
    {
        audioSource.clip = clip5;
        audioSource.Play();
    }
}

