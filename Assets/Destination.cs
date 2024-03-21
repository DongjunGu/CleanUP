using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destination : MonoBehaviour
{
    public GameObject correctObject;
    public GameObject effect;
    public static int destination_count = 0;

    private void Update()
    {
        float distance = Vector3.Distance(transform.position, correctObject.transform.position);
        if(distance < 1.0f)
        {
            effect.SetActive(true);
            destination_count++;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        //if (other.tag == "pushable" && other.name == correctObject.name)
        //{
           
        //}
        if(other.tag == "Player" && other.name == correctObject.name)
        {
            effect.SetActive(true);
        }
    }
}
