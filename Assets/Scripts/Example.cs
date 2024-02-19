using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Example : MonoBehaviour
{
    float maxJumpHeight = 3.0f;
    float groundHeight;
    Vector3 groundPos;
    public bool inputJump = false;
    public bool grounded = true;
    Animator anim;
    void Start()
    {
        anim = GetComponent<Animator>();
        groundPos = transform.position;
        groundHeight = transform.position.y;
        maxJumpHeight = transform.position.y + maxJumpHeight;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (grounded)
            {
                groundPos = transform.position;
                inputJump = true;
                StartCoroutine("Jumping");

            }
        }
        if (transform.position == groundPos)
            grounded = true;
        else
            grounded = false;
    }

    IEnumerator Jumping()
    {
        float playTime = 0.0f;
        float jumpHeight = 0.0f;
        Vector3 orgPos = transform.position;

        while (playTime < 1.0f)
        {
            jumpHeight = Mathf.Sin(playTime * Mathf.PI) * 3.0f;
            transform.position = orgPos + Vector3.up * jumpHeight;
            playTime += Time.deltaTime;

            yield return null;
        }
    }
}

