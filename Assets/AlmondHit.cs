using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlmondHit : MonoBehaviour
{
    public AudioClip clipAlmond;
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Obstacle")
        {
            SoundController.Instance.PlaySoundDesk("Almonds", clipAlmond);
            transform.gameObject.tag = "AlmondDropPos";
        }
    }
}
