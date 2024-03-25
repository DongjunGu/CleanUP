using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Pushable : MonoBehaviour
{
    public GameObject textMeshPro;
    public GameObject player;
    public GameObject remy;
    public Transform destination;
    private float distanceThreshold = 1.0f;
    private bool isPlayerChild = false;
    private Animator anim;
    public Vector3 orignalPosition;
    public static int count;
    public static bool allBlockSet = false;
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
    }
    void AllBlockSet()
    {
        if (count == 3)
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
            if (textMeshPro != null)
            {
                textMeshPro.gameObject.SetActive(true);
            }

            Transform playerTransform = transform.Find("player");

            if (playerTransform != null)
            {
                anim.SetBool("playPush", true);
                if (isCrash == false && Input.GetKey(KeyCode.W))
                {
                    anim.SetBool("isPush", true);
                    Vector3 moveDirection = remy.transform.forward.normalized;
                    float offset = 1.0f;
                    float dist = 3.0f * Time.deltaTime;
                    GameObject childObject = gameObject.transform.GetChild(0).gameObject;
                    float halfSize = childObject.GetComponent<BoxCollider>().size.x * 0.75f;

                    
                    Vector3 rightDir = Quaternion.Euler(0, 90, 0) * moveDirection;
                    Vector3 leftDir = Quaternion.Euler(0, -90, 0) * moveDirection;
                    Vector3 raycastOrigin = transform.position + Vector3.up * halfSize;
                    Vector3 raycastOrigin1 = raycastOrigin + rightDir * halfSize;
                    Vector3 raycastOrigin2 = raycastOrigin + leftDir * halfSize;
                    Debug.DrawRay(raycastOrigin, moveDirection * 5f, Color.red);
                    Debug.DrawRay(raycastOrigin1, moveDirection * 5f, Color.green);
                    Debug.DrawRay(raycastOrigin2, moveDirection * 5f, Color.blue);

                    RaycastHit[] hits = Physics.RaycastAll(raycastOrigin, moveDirection, halfSize + dist, LayerMask.GetMask("Wall"));

                    if(hits.Length == 0)
                    {
                        transform.Translate(moveDirection * dist);
                    }
                    else
                    {
                        foreach (RaycastHit hit in hits)
                        {
                            if (hit.transform != transform)
                            {
                                //transform.position += moveDirection * (hit.distance - halfSize);
                                //break;
                                Debug.Log(hit.transform.gameObject.name);
                            }
                        }
                    }


                   
                    

                }
                else
                {
                    anim.SetBool("isPush", false);
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
