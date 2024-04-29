using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExitGame : MonoBehaviour
{
    public Image RespawnImage;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            RespawnImage.GetComponent<Image>().enabled = true;
            RespawnImage.GetComponent<Animator>().enabled = true;
        }
    }
}
