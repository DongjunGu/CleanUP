using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TelePort : MonoBehaviour
{
    public static bool teleported = false;
    public Transform spawnPoint;
    public GameObject player;
    public UnityEngine.Events.UnityEvent InBossRoom;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            NewPlayerController.stage = 4;
            teleported = true;
            other.transform.position = spawnPoint.position;
            player.GetComponent<Animator>().SetBool("isSpin", false);
            player.GetComponent<NewPlayerController>().enabled = true;
            InBossRoom?.Invoke();
        }
    }
}
