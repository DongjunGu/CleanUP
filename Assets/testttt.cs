using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testttt : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody>().velocity = Vector3.down * 50f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
