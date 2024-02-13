using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerContoller : MonoBehaviour
{
    [SerializeField] public float _speed = 10.0f;
    [SerializeField] public float _jump = 5.0f;

    public GameObject[] weapons;
    public bool[] hasWeapons;
    Animator anim;
    private float v = 0.0f;
    private float h = 0.0f;
    private float _rotateSpeed = 10.0f;

    //float Idle_Run_Ratio = 0;

    private Rigidbody playerRigidbody;

    bool _isJumping; //점프 상태
    bool _isDodge;
    bool _obtainItem;
    bool _swapItem1;
    bool _swapItem2;
    bool _attackKey;
    bool _isAttack;

    Vector3 dir;
    Vector3 dogeVec;

    GameObject getItem;
    Weapons orginWeapon;

    int orginWeaponIndex = -1;
    float _attackDelay;
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
        ObtainItem(); //TODO 아이템 주울때 모션 추가
        SwapWeapon(); //TODO 스왑 모션 추가
        Attack();
    }

    void GetInput()
    {
        v = Input.GetAxis("Vertical");
        h = Input.GetAxis("Horizontal");
        _obtainItem = Input.GetButtonDown("Grab");
        _swapItem1 = Input.GetButtonDown("SwapItem1");
        _swapItem2 = Input.GetButtonDown("SwapItem2");
        _attackKey = Input.GetButtonDown("Attack1");

    }
    void PlayerMove()
    {
        dir = new Vector3(h, 0, v).normalized;
        if (_isAttack)
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

    void SwapWeapon()
    {
        if (_swapItem1 && (!hasWeapons[0] || orginWeaponIndex == 0))
            return;
        if (_swapItem2 && (!hasWeapons[1] || orginWeaponIndex == 1))
            return;
        int weaponIndex = -1;
        if (_swapItem1) weaponIndex = 0;
        if (_swapItem2) weaponIndex = 1;

        if ((_swapItem1 || _swapItem2) && !_isJumping && !_isDodge)
        {
            if (orginWeapon != null)
                orginWeapon.gameObject.SetActive(false);


            orginWeaponIndex = weaponIndex;
            orginWeapon = weapons[weaponIndex].GetComponent<Weapons>();
            orginWeapon.gameObject.SetActive(true);

        }
    }

    void Attack()
    {
        if (orginWeapon == null)
            return;
        _attackDelay += Time.deltaTime;
        _isAttack = orginWeapon.attackSpeed < _attackDelay;

        if (_attackKey && _isAttack && !_isDodge)
        {
            orginWeapon.Attack();
            anim.SetTrigger("playWipe");
            _attackDelay = 0;
        }
    }
    /// <summary>
    /// hasWeapons 배열에 무기를 얻으면 true 체크하고 그 오브젝트 파괴
    /// </summary>
    void ObtainItem()
    {
        if(_obtainItem && getItem != null && !_isJumping && !_isDodge)
        {
            if(getItem.tag == "Weapon")
            {
                Items item = getItem.GetComponent<Items>();
                int weaponIndex = item.value;
                hasWeapons[weaponIndex] = true;

                Destroy(getItem);
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
            anim.SetBool("isJump", false);
        _isJumping = false;
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Weapon")
            getItem = other.gameObject;
        //Debug.Log(getItem.name);
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Weapon")
            getItem = null;
    }
}
