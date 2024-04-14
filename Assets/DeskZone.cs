using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeskZone : MonoBehaviour
{
    public GameObject SpringArm;
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            SpringArm.transform.localPosition = new Vector3(0f, 11f, -12f);
        }
    }
}
