using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnZone : MonoBehaviour
{
    public Transform playerRespawn;
    public GameObject player;
    private Animator anim;
    void Start()
    {
        anim = player.GetComponent<Animator>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            anim.SetBool("playPush", false);
            NewPlayerController newPlayerController = player.GetComponent<NewPlayerController>();

            if (newPlayerController != null)
            {
                player.transform.parent = null;
                newPlayerController.enabled = true;
                newPlayerController.StopChecking();
            }

            player.transform.position = playerRespawn.position;
            
        }
    }
}
