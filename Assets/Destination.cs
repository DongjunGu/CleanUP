using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destination : MonoBehaviour
{
    public GameObject correctObject;
    public GameObject effect;
    public AudioClip clipCubeMatch;
    bool isMatch = false;
    private void Update()
    {
        float distance = Vector3.Distance(transform.position, correctObject.transform.position);
        if (distance < 1.0f && !isMatch)
        {
            effect.SetActive(true);
            ActiveSound();
        }


    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" && other.name == correctObject.name)
        {
            effect.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" && other.name == correctObject.name)
        {
            effect.SetActive(false);
        }
    }
    void ActiveSound()
    {
        if (!gameObject.name.Contains("player"))
        {
            isMatch = true;
            SoundController.Instance.PlaySound("Match", clipCubeMatch);
        }
            

    }
}
