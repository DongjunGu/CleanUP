using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PushCubeUI : MonoBehaviour
{
    public Image Eimage;
    public Transform remy;
    private void Update()
    {

    }
    private void OnTriggerStay(Collider other)
    {
        Transform player = transform.Find("player");
        if (other.tag == "Player")
        {
            if (player == null)
            {
                Eimage.gameObject.SetActive(true);
                Eimage.transform.position = Camera.main.WorldToScreenPoint(remy.position + Vector3.forward * 3f);
            }
            else if (player != null)
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