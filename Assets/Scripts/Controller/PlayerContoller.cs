using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerContoller : MonoBehaviour
{
    [SerializeField] private float _speed = 10.0f;
    [SerializeField] private float _jump = 3.0f;

    Animator anim;
    private float v = 0.0f;
    private float h = 0.0f;
    private float _rotateSpeed = 10.0f;

    float Idle_Run_Ratio = 0;

    private Rigidbody playerRigidbody;

    bool _isJumping; //점프 상태
    bool _isDodge;

    Vector3 dir;
    Vector3 dogeVec;
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
        PlayerDodge();
    }

    void GetInput()
    {
        v = Input.GetAxis("Vertical");
        h = Input.GetAxis("Horizontal");
    }
    void PlayerMove()
    {
        dir = new Vector3(h, 0, v).normalized;

        //Move
        if (!(v==0 && h == 0))
        {
            anim.SetBool("isRun", true);
            if (_isDodge)
                dir = dogeVec;
            transform.position += dir * _speed * Time.deltaTime;

            
        }
        else
        {
            anim.SetBool("isRun", false);
        }

        if (_isDodge)
            dir = dogeVec;

        //Rotate
        if (dir != Vector3.zero)
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * _rotateSpeed);

        //Idle-Run Blending
        //if (dir != Vector3.zero)
        //{
        //    Idle_Run_Ratio = Mathf.Lerp(Idle_Run_Ratio, 1, 10.0f * Time.deltaTime);
        //    anim.SetFloat("Idle_Run_Ratio", Idle_Run_Ratio);
        //    anim.Play("Idle_Run");
        //}
        //else
        //{
        //    Idle_Run_Ratio = Mathf.Lerp(Idle_Run_Ratio, 0, 10.0f * Time.deltaTime);
        //    anim.SetFloat("Idle_Run_Ratio", Idle_Run_Ratio);
        //    anim.Play("Idle_Run");
        //}
    }

    void PlayerJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !_isJumping && !_isDodge)
        {
            playerRigidbody.AddForce(Vector3.up * _jump, ForceMode.Impulse);
            anim.SetBool("isJump", true);
            anim.SetTrigger("playJump");
            _isJumping = true;
        }
    }

    void PlayerDodge()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && !_isJumping && !_isDodge && dir != Vector3.zero)
        {
            playerRigidbody.AddForce(Vector3.up * _jump, ForceMode.Impulse);
            dogeVec = dir;
            anim.SetTrigger("playDodge");
            _isDodge = true;
            Invoke("FinishDodge", 1.0f);
        }
    }

    void FinishDodge()
    {
        _isDodge = false;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
            anim.SetBool("isJump", false);
        _isJumping = false;

    }
}
