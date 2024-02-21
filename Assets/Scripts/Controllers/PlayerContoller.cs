using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerContoller : MonoBehaviour
{
    [SerializeField] public float _speed = 10.0f;
    [SerializeField] LayerMask groundLayer = 1 << 9;
    [SerializeField] LayerMask wallLayer = 1 << 6;

    [SerializeField]
    private float rotCamXAxisSpeed = 5.0f;
    [SerializeField]
    private float rotCamYAxisSpeed = 3.0f;

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
    bool _isJumping;
    bool _isDodge;
    bool _obtainItem;
    bool _swapItem1;
    bool _swapItem2;
    bool _attackKey;
    bool _isAttack;
    bool _isGrounded;
    bool _isBorder; //충돌감지
    bool _isBorderDodge;
    bool _canDoubleJump = true;

    private bool _isWipeAnimationPlaying = false;

    Vector3 dir;
    Vector3 dogeVec;
    Vector3 initialJumpPosition;
    GameObject getItem;
    Weapons orginWeapon;

    int orginWeaponIndex = -1;
    float _attackDelay;

    private float limitMinX = -80;
    private float limitMaxX = 50;
    private float eulerAngleX;
    private float eulerAngleY;
    private Vector3 lastPosition;
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
        CheckGrounded();

        //UpdateRotate();
        //UpdateMove();
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
        dir = new Vector3(h, 0, v).normalized;
        if (_isAttack)
            dir = new Vector3(h, 0, v).normalized;
        //Move
        if (!(v == 0 && h == 0))
        {
            //if (_isDodge)
            //    dir = dogeVec;
            anim.SetBool("isRun", true);
            if (!_isBorder)
                transform.position += dir * _speed * Time.deltaTime;
        }
        else
        {
            anim.SetBool("isRun", false);
        }

        //Rotate
        if (dir != Vector3.zero)
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * _rotateSpeed);
    }

    void PlayerJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && (!_isJumping || (_canDoubleJump && !_isDodge)))
        {
            if (_isJumping)
            {
                _canDoubleJump = false;
                anim.SetTrigger("playDoubleJump");
                anim.SetBool("isDoubleJumping", true);
                playerRigidbody.AddForce(Vector3.up * 10.0f, ForceMode.Impulse);

                return;
            }
            playerRigidbody.AddForce(Vector3.up * 10.0f, ForceMode.Impulse);
            anim.SetBool("isJumping", true);
            _isJumping = true;
            anim.SetTrigger("playJump");

        }
    }

    void PlayFall()
    {
        anim.SetTrigger("playFall");
        _isJumping = true;
    }

    void PlayerDodge()
    {
        //if (Input.GetKeyDown(KeyCode.LeftShift) && !_isBorderDodge && !_isJumping && !_isDodge && dir != Vector3.zero)
        //if (Input.GetKeyDown(KeyCode.LeftShift) && !_isBorderDodge && !_isJumping && !_isDodge && !(h == 0 && v == 0))
        if (Input.GetKeyDown(KeyCode.LeftShift) && !_isBorderDodge /*&& !_isJumping*/ && !_isDodge && dir != Vector3.zero)
        {
            dogeVec = dir;
            RaycastHit hit;
            if (Physics.Raycast(transform.position, dogeVec, out hit, _speed * 1.5f, LayerMask.GetMask("Wall")))
            {
                // If the ray hits a wall, adjust the dodge direction to be away from the wall
                return;
            }
            anim.SetTrigger("playDodge");
            _isDodge = true;
            _isJumping = true;
            Invoke("FinishDodge", 0.8f);
        }
    }

    void FinishDodge()
    {
        _isDodge = false;
        _isJumping = false;
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

    //void Attack()
    //{
    //    if (orginWeapon == null)
    //        return;
    //    _attackDelay += Time.deltaTime;
    //    _isAttack = orginWeapon.attackSpeed < _attackDelay;

    //    if (_attackKey && _isAttack && !_isDodge && !_isJumping)
    //    {
    //        orginWeapon.Attack();
    //        anim.SetTrigger("playWipe");
    //        _attackDelay = 0.0f;
    //        //공격하는 동안 점프 불가
    //    }

    //}
    void Attack()
    {
        if (orginWeapon == null)
            return;

        _attackDelay += Time.deltaTime;
        _isAttack = orginWeapon.attackSpeed < _attackDelay;

        if (_attackKey && _isAttack && !_isDodge && !_isJumping && !_isWipeAnimationPlaying)
        {
            StartCoroutine(PlayWipeAnimation());
        }
    }
    IEnumerator PlayWipeAnimation()
    {
        _isWipeAnimationPlaying = true;

        orginWeapon.Attack();
        anim.SetTrigger("playWipe");
        _attackDelay = 0.0f;

        // Disable jumping during the attack
        _isJumping = true;
        anim.SetBool("isJumping", false);
        _canDoubleJump = false;
        anim.SetBool("isDoubleJumping", false);

        
        yield return new WaitForSeconds(0.9f);

        // Enable jumping after the wipe animation
        _isJumping = false;
        _canDoubleJump = true;
        anim.SetBool("isDoubleJumping", false);

        _isWipeAnimationPlaying = false;
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

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            anim.SetBool("isJumping", false);
            _isJumping = false;
            _canDoubleJump = true;
            anim.SetBool("isDoubleJumping", false);
            //anim.SetBool("isGrounded", true);
        }

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
    void CheckGrounded()
    {
        RaycastHit hit;
        float raycastDistance = 0.5f;
        Vector3 raycastOrigin = transform.position + Vector3.up * 0.1f;
        anim.ResetTrigger("playFall");
        LayerMask combinedLayers = groundLayer | wallLayer;

        _isGrounded = Physics.Raycast(raycastOrigin, Vector3.down, out hit, raycastDistance, combinedLayers);

        Debug.DrawRay(raycastOrigin, Vector3.down * raycastDistance, _isGrounded ? Color.green : Color.red);
        anim.SetBool("isGrounded", true);
        if (!_isGrounded)
        {
            anim.SetBool("isGrounded", false);
            PlayFall();
        }
    }
    void RotationFreeze()
    {
        playerRigidbody.angularVelocity = Vector3.zero;
    }

    void StopBeforeObject() //충돌전 확인
    {
        //Debug.DrawRay(transform.position + Vector3.up, transform.forward * 3, Color.green);
        _isBorder = Physics.Raycast(transform.position + Vector3.up, transform.forward, 2, LayerMask.GetMask("Wall"));
        _isBorderDodge = Physics.Raycast(transform.position + Vector3.up, transform.forward, 5, LayerMask.GetMask("Wall"));
    }
    private void FixedUpdate()
    {
        RotationFreeze();
        StopBeforeObject();
    }

    void UpdateRotate()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        eulerAngleY += mouseX * rotCamXAxisSpeed;
        eulerAngleX -= mouseY * rotCamYAxisSpeed;

        eulerAngleX = ClampAngle(eulerAngleX, limitMinX, limitMaxX);

        transform.rotation = Quaternion.Euler(eulerAngleX, eulerAngleY, 0);

    }

    private void UpdateMove()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 dir = new Vector3(h, 0, v).normalized;
        if (_isAttack)
            dir = new Vector3(h, 0, v).normalized;
        //Vector3 moveVector = dir * _speed * Time.deltaTime;
        if (!(h == 0 && v == 0))
        {

            anim.SetBool("isRun", true);
            if (_isDodge)
                dir = dogeVec;
            //Quaternion targetRotation = Quaternion.LookRotation(dir, Vector3.up);
            //transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * _rotateSpeed);
            if (!_isBorder)
                transform.Translate(dir * _speed * Time.deltaTime, Space.Self);
            //transform.position += moveVector;
        }
        else
        {
            anim.SetBool("isRun", false);
        }


        //Vector3 dir = transform.rotation * new Vector3(x, 0, z);

        //Vector3 moveForce = new Vector3(dir.x * moveSpeed, 0, dir.z * moveSpeed);

        //characterController.Move(moveForce * Time.deltaTime);
    }
    private float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360) angle += 360;
        if (angle > 360) angle -= 360;

        return Mathf.Clamp(angle, min, max);
    }

}
