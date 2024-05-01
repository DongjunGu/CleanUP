using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveParticleSound : MonoBehaviour
{
    private void OnEnable()
    {
        Invoke("ActiveSound", 1f);
    }

    void ActiveSound()
    {
        GetComponent<AudioSource>().Play();
    }
}
