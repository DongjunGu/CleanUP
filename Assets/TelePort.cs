using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TelePort : MonoBehaviour
{
    public static bool teleported = false;
    public Transform spawnPoint;
    public GameObject player;
    private void OnTriggerEnter(Collider other)
    {
        teleported = true;
        other.transform.position = spawnPoint.position;
        player.GetComponent<Animator>().SetBool("isSpin", false);
        player.GetComponent<NewPlayerController>().enabled = true;
    }
}
