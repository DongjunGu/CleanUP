using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerContoller : MonoBehaviour
{
    [SerializeField]
    private float _speed;
    [SerializeField]
    private float _jump;

    private float v = 0.0f;
    private float h = 0.0f;

    private Transform _playerTr;
    private float rotateSpeed = 10.0f;
    private Rigidbody _playerRigidbody;

    bool _isJumping = false; //점프 상태
    void Start()
    {
        _playerTr = GetComponent<Transform>();
        _playerRigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        PlayerMove();
        PlayerJump();

    }

    void PlayerMove()
    {
        v = Input.GetAxis("Vertical");
        h = Input.GetAxis("Horizontal");

        Vector3 dir = new Vector3(h, 0, v);

        //Move
        transform.position += dir * _speed * Time.deltaTime;
        //Rotate
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * rotateSpeed);
    }

    void PlayerJump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!_isJumping)
            {
                _isJumping = true;
                _playerRigidbody.AddForce(Vector3.up * _jump, ForceMode.Impulse);
            }
            else
                return;
        }

    }
    private void OnCollisonEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            _isJumping = false;
    }
}
