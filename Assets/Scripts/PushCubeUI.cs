using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PushCubeUI : MonoBehaviour
{
    public Transform target12;
    public Transform target3;
    public Transform target6;
    public Transform target9;
    public Image Eimage;
    GameObject prefab;
    GameObject EPrefab;
    bool keyPressed = false;
    int count = 2;
    void Start()
    {
        
    }

    private void Update()
    {
        
    }
    private void OnTriggerStay(Collider other)
    {
        Transform player = transform.Find("player");
        if (other.tag == "Player")
        {
            if(player == null)
            {
                if (NewPlayerController.direction == 1)
                {
                    Eimage.gameObject.SetActive(true);
                    Eimage.transform.position = Camera.main.WorldToScreenPoint(target12.position);
                }

                if (NewPlayerController.direction == 2)
                {
                    Eimage.gameObject.SetActive(true);
                    Eimage.transform.position = Camera.main.WorldToScreenPoint(target3.position);
                }

                if (NewPlayerController.direction == 3)
                {
                    Eimage.gameObject.SetActive(true);
                    Eimage.transform.position = Camera.main.WorldToScreenPoint(target6.position);
                }

                if (NewPlayerController.direction == 4)
                {
                    Eimage.gameObject.SetActive(true);
                    Eimage.transform.position = Camera.main.WorldToScreenPoint(target9.position);
                }
                keyPressed = false;
            }
            else if(player != null)
            {
                Eimage.gameObject.SetActive(false);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            Eimage.gameObject.SetActive(false);
        }
    }

}
