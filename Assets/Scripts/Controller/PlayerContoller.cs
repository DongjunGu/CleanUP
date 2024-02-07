using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerContoller : MonoBehaviour
{
    [SerializeField] private float _speed = 10.0f;
    //[SerializeField] private float _jump = 5.0f;

    Animator anim;
    private float v = 0.0f;
    private float h = 0.0f;
    private float _rotateSpeed = 10.0f;

    float Idle_Run_Ratio = 0;

    private Rigidbody playerRigidbody;

    bool _isJumping; //점프 상태
    void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        GetInput();
        PlayerMove();
        PlayerJump();

    }

    void GetInput()
    {
        v = Input.GetAxis("Vertical");
        h = Input.GetAxis("Horizontal");
    }
    void PlayerMove()
    {

        Vector3 dir = new Vector3(h, 0, v).normalized;

        //Move
        transform.position += dir * _speed * Time.deltaTime;

        //Rotate
        if (dir != Vector3.zero)
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * _rotateSpeed);

        //Idle-Run Blending
        if (dir != Vector3.zero)
        {
            Idle_Run_Ratio = Mathf.Lerp(Idle_Run_Ratio, 1, 10.0f * Time.deltaTime);
            anim.SetFloat("Idle_Run_Ratio", Idle_Run_Ratio);
            anim.Play("Idle_Run");
        }
        else
        {
            Idle_Run_Ratio = Mathf.Lerp(Idle_Run_Ratio, 0, 10.0f * Time.deltaTime);
            anim.SetFloat("Idle_Run_Ratio", Idle_Run_Ratio);
            anim.Play("Idle_Run");
        }
    }

    void PlayerJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !_isJumping)
        {
            playerRigidbody.AddForce(Vector3.up * 5, ForceMode.Impulse);
            _isJumping = true;
        }

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
            _isJumping = false;
    }
}
