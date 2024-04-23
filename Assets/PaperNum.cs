using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperNum : MonoBehaviour
{
    public static int Papernumber;
    private void Start()
    {
        
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            Papernumber = 3;
        }
    }

}
