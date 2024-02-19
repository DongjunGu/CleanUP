using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKinemetic : MonoBehaviour
{
    [SerializeField] public float _speed = 10.0f;
    [SerializeField] LayerMask groundLayer = 1 << 9;
    public GameObject[] weapons;
    public bool[] hasWeapons;
    Animator anim;
    private float v = 0.0f;
    private float h = 0.0f;
    private float _rotateSpeed = 10.0f;

    //float Idle_Run_Ratio = 0;

    private Rigidbody playerRigidbody;

    //bool _isJumping
    //{
    //    get => anim.GetBool("isJumping");
    //}
    bool _isGrounded;
    bool _isJumping;
    bool _isDodge;
    bool _obtainItem;
    bool _swapItem1;
    bool _swapItem2;
    bool _attackKey;
    bool _isAttack;
    //bool _isFalling;
    bool _isBorder; //충돌감지
    bool _canDoubleJump = true;

    Vector3 dir;
    Vector3 dogeVec;
    Vector3 orgPos;
    Vector3 initialJumpPosition;
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
        PlayerDodge();
        PlayerJump();
        //PlayerFall();
        ObtainItem(); //TODO 아이템 주울때 모션 추가
        SwapWeapon(); //TODO 스왑 모션 추가
        Attack();
        CheckGrounded();

    }
    void CheckGrounded()
    {
        RaycastHit hit;
        float raycastDistance = 0.5f;
        Vector3 raycastOrigin = transform.position + Vector3.up * 0.1f;


        _isGrounded = Physics.Raycast(raycastOrigin, Vector3.down, out hit, raycastDistance, groundLayer);


        Debug.DrawRay(raycastOrigin, Vector3.down * raycastDistance, _isGrounded ? Color.green : Color.red);
    }
    void GetInput()
    {
        v = Input.GetAxisRaw("Vertical");
        h = Input.GetAxisRaw("Horizontal");
        _obtainItem = Input.GetButtonDown("Grab");
        _swapItem1 = Input.GetButtonDown("SwapItem1");
        _swapItem2 = Input.GetButtonDown("SwapItem2");
        _attackKey = Input.GetButtonDown("Attack1");

    }
    void PlayerMove()
    {
        if (_isAttack)
            dir = new Vector3(h, 0, v).normalized;
        //Move
        dir = new Vector3(h, 0, v).normalized;

        if (!(v == 0 && h == 0))
        {
            anim.SetBool("isRun", true);

            if (_isDodge)
                dir = dogeVec;

            if (!_isBorder)
            {
                transform.position += dir * _speed * Time.deltaTime;
            }


        }
        else
        {
            anim.SetBool("isRun", false);
        }

        if (_isDodge)
            dir = dogeVec;
        // Rotate
        if (dir != Vector3.zero)
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * _rotateSpeed);
    }
    void PlayerJump()
    {
        if (_isGrounded)
        {
            // On the ground
            _canDoubleJump = true;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                anim.SetTrigger("playJump");
                StartCoroutine(Jumping());
            }

            anim.SetBool("isGrounded", true);
        }
        else if (_canDoubleJump)
        {

            if (Input.GetKeyDown(KeyCode.Space))
            {
                anim.SetTrigger("playDoubleJump");
                StartCoroutine(Jumping());

                _canDoubleJump = false;
            }
        }
        anim.SetBool("isGrounded", true);

    }
    //IEnumerator Jumping()
    //{
    //    float playTime = 0.0f;
    //    float jumpHeight = 0.0f;
    //    orgPos = transform.position;

    //    _isJumping = true;
    //    anim.SetBool("isJumping", true);
    //    if (_isGrounded || _canDoubleJump)
    //    {
    //        if (_isGrounded)
    //        {
    //            _canDoubleJump = false;
    //            anim.SetBool("isDoubleJumping", true);
    //            anim.SetTrigger("playDoubleJump");

    //        }
    //        while (playTime < 1.0f)
    //        {
    //            jumpHeight = Mathf.Sin(playTime * Mathf.PI) * 4.0f;
    //            transform.position = orgPos + Vector3.up * jumpHeight;
    //            playTime += Time.deltaTime;
    //            anim.SetBool("isGrounded", false);
    //            yield return null;
    //        }
    //        //transform.position = new Vector3(orgPos.x, orgPos.y, orgPos.z);
    //        transform.position = orgPos;
    //        _isJumping = false;
    //        _canDoubleJump = true;
    //        anim.SetBool("isJumping", false);
    //        anim.SetBool("isDoubleJumping", false);
    //    }
    //}
    IEnumerator Jumping()
    {
        float playTime = 0.0f;
        float jumpHeight = 0.0f;
        orgPos = transform.position;

        _isJumping = true;
        anim.SetBool("isJumping", true);

        if (_isGrounded || _canDoubleJump) // Check if it's the first jump or double jump
        {
            if (!_isGrounded)
            {
                _canDoubleJump = false;
                anim.SetBool("isDoubleJumping", true);
            }

            while (playTime < 1.0f)
            {
                jumpHeight = Mathf.Sin(playTime * Mathf.PI) * 4.0f;
                transform.position = orgPos + Vector3.up * jumpHeight;
                playTime += Time.deltaTime;
                anim.SetBool("isGrounded", false);
                yield return null;
            }
        }

        // Reset the animation states and position
        transform.position = orgPos;
        _isJumping = false;
        anim.SetBool("isJumping", false);
        anim.SetBool("isDoubleJumping", false);
        anim.SetBool("isGrounded", true);
    }
    void PlayFall()
    {
        anim.SetTrigger("playFall");
    }
    //void PlayerFall()
    //{
    //    if (!_isJumping && !_isDodge && !_isFalling)
    //    {
    //        RaycastHit hit;
    //        float raycastDistance = 0.5f;
    //        Vector3 raycastOrigin = transform.position + Vector3.up * 0.1f;

    //        // 레이캐스트를 player의 아래 방향으로 쏘아 충돌을 감지
    //        if (!Physics.Raycast(raycastOrigin, Vector3.down, out hit, raycastDistance))
    //        {
    //            if (!isFallingTriggered) // isFallingTriggered가 false일 때만 실행
    //            {
    //                anim.SetTrigger("playFall");
    //                isFallingTriggered = true;
    //            }
    //            _isFalling = true;
    //        }
    //        else
    //        {
    //            _isFalling = false;
    //            isFallingTriggered = false; // 충돌이 감지되면 리셋
    //        }
    //    }
    //}

    void PlayerDodge()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && !_isJumping && !_isDodge && dir != Vector3.zero)
        {

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
        if (_obtainItem && getItem != null && !_isJumping && !_isDodge)
        {
            if (getItem.tag == "Weapon")
            {
                Items item = getItem.GetComponent<Items>();
                int weaponIndex = item.value;
                hasWeapons[weaponIndex] = true;

                Destroy(getItem);
            }
        }
    }

    private void OnAnimatorMove()
    {
        transform.Translate(anim.deltaPosition, Space.World);
        transform.rotation *= anim.deltaRotation;
    }


    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Weapon")
            getItem = other.gameObject;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Weapon")
            getItem = null;
    }

    void RotationFreeze()
    {
        playerRigidbody.angularVelocity = Vector3.zero;
    }

    void StopBeforeObject() //충돌전 확인
    {
        //Debug.DrawRay(transform.position + Vector3.up, transform.forward * 3, Color.green);
        _isBorder = Physics.Raycast(transform.position + Vector3.up, transform.forward, 2, LayerMask.GetMask("Wall"));
    }
    private void FixedUpdate()
    {
        RotationFreeze();
        StopBeforeObject();
    }

}
