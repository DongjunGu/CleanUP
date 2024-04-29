using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Pushable : MonoBehaviour
{
    public GameObject player;
    public GameObject remy;
    public Transform destination;
    private float distanceThreshold = 1.0f;
    private bool isPlayerChild = false;
    private Animator anim;
    public Vector3 orignalPosition;
    public static int count = 0;
    public static bool allBlockSet = false;
    public AudioClip clipPush;
    bool isMoving = false;
    bool isPlaying = false;
    AudioSource pushAudio;
    private void Awake()
    {
        pushAudio = GetComponent<AudioSource>();
    }
    void Start()
    {
        anim = player.GetComponent<Animator>();
        orignalPosition = transform.position;
    }
    void Update()
    {
        distachPlayer();
        ArrivedDestination();
        AllBlockSet();
        if(isMoving)
            CubeMoving();
    }

    void CubeMoving()
    {
        if (Input.GetKey(KeyCode.W))
        {
            pushAudio.mute = false;
            //pushAudio.Play();
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            pushAudio.mute = true;

        }
    }
    void AllBlockSet()
    {
        if (count == 7) //Test, should be 7
            allBlockSet = true;
    }

    void ArrivedDestination()
    {
        float distance = Vector3.Distance(transform.position, destination.position);
        if (distance < distanceThreshold)
        {
            distachPlayerFromParent();
            gameObject.GetComponent<Collider>().enabled = false;
            gameObject.GetComponent<Pushable>().enabled = false;
            pushAudio.enabled = false;
            count++;
            Debug.Log(count);
        }
    }
    void distachPlayer()
    {
        if (isPlayerChild && Input.GetKeyDown(KeyCode.E))
        {
            distachPlayerFromParent();
        }
    }
    
    public void distachPlayerFromParent()
    {
        anim.SetBool("playPush", false);
        isPlayerChild = false;
        NewPlayerController newPlayerController = player.GetComponent<NewPlayerController>();

        if (newPlayerController != null)
        {
            player.transform.parent = null;
            newPlayerController.enabled = true;
            newPlayerController.StopChecking();
        }
    }

    bool isCrash = false;
    public void OnCrash(bool v)
    {
        isCrash = v;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Respawn")
        {
            distachPlayerFromParent();
            transform.position = orignalPosition;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player")
        {

            Transform playerTransform = transform.Find("player");

            if (playerTransform != null)
            {
                anim.SetBool("playPush", true);
                if (isCrash == false && Input.GetKey(KeyCode.W))
                {
                    anim.SetBool("isPush", true);
                    Vector3 moveDirection = remy.transform.forward.normalized;
                    float dist = 3.0f * Time.deltaTime;
                    GameObject childObject = gameObject.transform.GetChild(0).gameObject;
                    float halfSize = childObject.GetComponent<BoxCollider>().size.x * 0.8f;

                    
                    Vector3 rightDir = Quaternion.Euler(0, 90, 0) * moveDirection;
                    Vector3 leftDir = Quaternion.Euler(0, -90, 0) * moveDirection;
                    Vector3[] raycastOrigin = new Vector3[3];
                    raycastOrigin[0] = transform.position + Vector3.up * halfSize;
                    raycastOrigin[1] = raycastOrigin[0] + rightDir * halfSize;
                    raycastOrigin[2] = raycastOrigin[0] + leftDir * halfSize;


                    bool check = false;
                    for(int i = 0; i < 3; ++i)
                    {
                        RaycastHit[] hits = Physics.RaycastAll(raycastOrigin[i], moveDirection, halfSize + dist, LayerMask.GetMask("Wall"));
                        if(hits.Length > 0) 
                        { 
                            check = true;
                            break;
                        }
                    }

                    if(check == false)
                    {
                        transform.Translate(moveDirection * dist);
                        isMoving = true;
                        //Debug.Log("push");
                        //SoundController.Instance.PlaySound("Push", clipPush);
                    }


                }
                else
                {
                    anim.SetBool("isPush", false);
                    isMoving = false;
                    pushAudio.mute = true;
                }

                player.transform.parent = transform;
                isPlayerChild = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {

    }

}
