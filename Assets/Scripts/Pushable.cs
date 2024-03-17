using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Pushable : MonoBehaviour
{
    public GameObject textMeshPro;
    public GameObject player;
    public GameObject remy;
    private bool isPlayerChild = false;
    private Animator anim;
    bool stopPush;
    void Start()
    {
        anim = player.GetComponent<Animator>();
    }
    void Update()
    {
        distachPlayer();
    }
    void distachPlayer()
    {
        if (isPlayerChild && Input.GetKeyDown(KeyCode.E) && stopPush)
        {
            anim.SetBool("playPush", false);
            isPlayerChild = false;
            NewPlayerController newPlayerController = player.GetComponent<NewPlayerController>();

            if (newPlayerController != null)
            {
                player.transform.parent = null;
                newPlayerController.enabled = true;
            } 
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player")
        {
            if (textMeshPro != null)
            {
                textMeshPro.gameObject.SetActive(true);
            }

            Transform playerTransform = transform.Find("player");

            if (playerTransform != null)
            {
                anim.SetBool("playPush", true);
                if (Input.GetKey(KeyCode.W))
                {
                    anim.SetBool("isPush", true);
                    stopPush = false;
                    Vector3 moveDirection = remy.transform.forward.normalized;
                    transform.Translate(moveDirection * 3.0f * Time.deltaTime);
                }
                else
                {
                    anim.SetBool("isPush", false);
                    stopPush = true;
                }

                player.transform.parent = transform;
                isPlayerChild = true;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (textMeshPro != null)
        {
            textMeshPro.gameObject.SetActive(false);
        }
    }

}
