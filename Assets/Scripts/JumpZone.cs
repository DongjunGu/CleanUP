using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpZone : MonoBehaviour
{
    bool isJumpZone = false;
    new Rigidbody rigidbody;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "JumpZone")
        {
            rigidbody.AddForce(Vector3.up * 100.0f, ForceMode.Impulse);
            isJumpZone = true;
            if (isJumpZone)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    rigidbody.AddForce(Vector3.up * 100.0f, ForceMode.Impulse);
                }
            }
        }
    }

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        JumpMovement();
    }
    
    void JumpMovement()
    {
        if(isJumpZone)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                rigidbody.AddForce(Vector3.up * 100.0f, ForceMode.Impulse);
            }
        }
        isJumpZone = false;
    }
}
