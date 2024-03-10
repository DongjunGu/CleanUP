using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewPlayerController : MonoBehaviour
{
    [SerializeField] public float _speed = 10.0f;
    [SerializeField] LayerMask groundLayer = 1 << 9;
    [SerializeField] LayerMask wallLayer = 1 << 6;
    [SerializeField] LayerMask obstacleLayer = 1 << 15;
    [SerializeField] GameObject weaponImage1;
    [SerializeField] GameObject weaponImage2;
    [SerializeField] Transform player;
    [SerializeField] public float _rotateSpeed = 5.0f;
    public Camera mainCamera;
    public GameObject[] weapons;
    public bool[] hasWeapons;

    public GameObject[] dusts;
    public int hasDust;

    public int maxDust;
    public GameObject dustObject;

    Animator anim;
    private float v = 0.0f;
    private float h = 0.0f;
    //private float _rotateSpeed = 10.0f;

    private Rigidbody playerRigidbody;

    bool _isJumping;
    bool _isDodge;
    bool _obtainItem;
    bool _swapItem1;
    bool _swapItem2;
    bool _isSwap;
    bool _attackKey;
    bool _isAttack;
    bool _dustAttack;
    bool _isGrounded;
    bool _canDoubleJump = true;
    bool isJumpZone = false;

    bool wasGrounded = true;
    private bool _isWipeAnimationPlaying = false;

    Vector3 dir;
    Vector3 dogeVec;
    Vector3 _velocity;
    GameObject getItem;
    GameObject getImage;
    Weapons orginWeapon;

    int orginWeaponIndex = -1;
    float _attackDelay;
    void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        mainCamera = GetComponentInChildren<Camera>();
    }
    void Update()
    {
        GetInput();
        PlayerJump();
        PlayerDodge();
        ObtainItem(); //TODO 아이템 주울때 모션 추가
        SwapWeapon();
        Attack();
        Dust();
        CheckGrounded();
    }
    void GetInput()
    {
        v = Input.GetAxisRaw("Vertical");
        h = Input.GetAxisRaw("Horizontal");
        _obtainItem = Input.GetButtonDown("Grab");
        _swapItem1 = Input.GetButtonDown("SwapItem1");
        _swapItem2 = Input.GetButtonDown("SwapItem2");
        _attackKey = Input.GetButtonDown("Attack1");
        _dustAttack = Input.GetButtonDown("Attack2");

    }
    void PlayerMove()
    {
        dir = new Vector3(h, 0, v).normalized;

        if (_isAttack)
            dir = new Vector3(h, 0, v).normalized;

        if (!(v == 0 && h == 0))
        {
            Vector3 _moveHorizontal = transform.right * h;
            Vector3 _moveVertical = transform.forward * v;
            float dist = _speed * Time.deltaTime;
            _velocity = (_moveHorizontal + _moveVertical).normalized;

            anim.SetBool("isRun", true);
            float offset = 1.0f;
            if (Physics.Raycast(new Ray(transform.position - _velocity * offset, _velocity), out RaycastHit hit, dist + offset * 2.0f, LayerMask.GetMask("Wall")))
            {
                dist = hit.distance - offset * 2.0f;
            }
            transform.Translate(_velocity * dist, Space.World);

            Quaternion targetRotation = Quaternion.LookRotation(new Vector3(_velocity.x, 0, _velocity.z), Vector3.up);
            player.rotation = Quaternion.Slerp(player.rotation, targetRotation, Time.deltaTime * _rotateSpeed);
        }
        else
        {
            anim.SetBool("isRun", false);
        }

        float mouseX = Input.GetAxis("Mouse X");
        transform.Rotate(Vector3.up * mouseX * _rotateSpeed);
    }
    void PlayerJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !_isSwap && (!_isJumping || (_canDoubleJump && !_isDodge)))
        {
            if (_isJumping && _canDoubleJump)
            {
                _canDoubleJump = false;
                anim.SetTrigger("playDoubleJump");
                playerRigidbody.AddForce(Vector3.up * 14.0f, ForceMode.Impulse);
                Invoke("FallAfterJump", 0.3f);
                Debug.Log("JUMP3");
                return;
            }

            if (isJumpZone)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    playerRigidbody.velocity = Vector3.zero;
                    playerRigidbody.AddForce(Vector3.up * 20.0f, ForceMode.Impulse);
                    _isJumping = true;
                    anim.SetBool("isJumping", true);
                    anim.SetTrigger("playJump");
                    _canDoubleJump = true;
                    Debug.Log("JUMP2");
                    Invoke("FallAfterJump", 0.3f);
                    return;

                }
            }
            else
            {
                playerRigidbody.velocity = Vector3.zero;
                playerRigidbody.AddForce(Vector3.up * 15.0f, ForceMode.Impulse);
                anim.SetBool("isJumping", true);
                _isJumping = true;
                anim.SetTrigger("playJump");
                _canDoubleJump = true;
                Debug.Log("JUMP");
                Invoke("FallAfterJump", 0.3f);
                return;
            }
        }
    }
    void FallAfterJump()
    {
        playerRigidbody.AddForce(Vector3.down * 5.0f, ForceMode.Impulse);
    }
    void PlayFall()
    {
        anim.SetTrigger("playFall");
    }
    void PlayerDodge()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && !_isSwap && !_isDodge && dir != Vector3.zero && !_isJumping)
        {
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

            anim.SetTrigger("playSwap");
            _isSwap = true;
            Invoke("SwapFinish", 0.8f);

        }
    }
    void SwapFinish()
    {
        _isSwap = false;
    }
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

    void Dust()
    {
        if (hasDust == 0)
            return;

        if (_dustAttack && !_isJumping && !_isDodge && !_isSwap)
        {

            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100))
            {
                anim.SetTrigger("playThrow");



                Vector3 nextVec = hit.point - transform.position;
                nextVec.y = 10;

                Vector3 lookAtPos = Input.mousePosition;
                float lookAngle = Vector3.Angle(transform.forward, nextVec.normalized); // 사이각구하기

                if (Vector3.Dot(transform.right, nextVec.normalized) < 0) // 내적을 계산해서 0 : 직각, 음수 : 반시계
                {
                    transform.Rotate(Vector3.up * -lookAngle); //반시계
                }
                else
                {
                    transform.Rotate(Vector3.up * lookAngle); //시계
                }
                GameObject instantDust = Instantiate(dustObject, transform.position, transform.rotation);
                Rigidbody rigidDust = instantDust.GetComponent<Rigidbody>();
                rigidDust.AddForce(nextVec, ForceMode.Impulse);
                rigidDust.AddTorque(Vector3.back * 10, ForceMode.Impulse); //회전

                hasDust--;
            }

        }
    }
    IEnumerator PlayWipeAnimation()
    {
        _isWipeAnimationPlaying = true;

        orginWeapon.Attack();
        anim.SetTrigger("playWipe");
        _attackDelay = 0.0f;

        _isJumping = true;
        anim.SetBool("isJumping", false);
        _canDoubleJump = false;
        anim.SetBool("isDoubleJumping", false);


        yield return new WaitForSeconds(0.9f);

        _isJumping = false;
        _canDoubleJump = true;
        anim.SetBool("isDoubleJumping", false);

        _isWipeAnimationPlaying = false;
    }
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

                if (getItem.name == "Broom")
                {
                    weaponImage1.SetActive(true);
                }
                else if (getItem.name == "Hammer")
                {
                    weaponImage2.SetActive(true);
                }

            }
        }
    }
    private void OnAnimatorMove()
    {
        float offset = 0.5f;
        if (Physics.Raycast(new Ray(transform.position - anim.deltaPosition.normalized * offset, anim.deltaPosition.normalized), out RaycastHit hit,
            anim.deltaPosition.magnitude + offset * 2.0f, LayerMask.GetMask("Wall"))) //TODO Add Obstacle
        {
            transform.position += anim.deltaPosition.normalized * (hit.distance - offset * 2.0f);
        }
        else
        {
            //MEMO 역행렬로 회전
            Vector3 temp = transform.InverseTransformDirection(anim.deltaPosition);
            transform.Translate(player.rotation * temp , Space.World);
        }
        player.rotation *= anim.deltaRotation;
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            //anim.SetBool("isJumping", false);
            //_isJumping = false;
            isJumpZone = false;
        }
        if (collision.gameObject.tag == "JumpZone")
        {
            isJumpZone = true;
            anim.SetBool("isJumping", false);
            _isJumping = false;
            Debug.Log(collision.gameObject.tag);
        }

        if (collision.gameObject.tag == "CumpulsionJumpZone")
        {
            playerRigidbody.velocity = Vector3.zero;
            playerRigidbody.AddForce(Vector3.up * 30.0f, ForceMode.Impulse);
            Debug.Log("CumpulsionJump");
            Debug.Log(collision.gameObject.tag);
        }

        if (((1 << collision.gameObject.layer) & obstacleLayer) != 0) //Layer
        {
            //윗부분만 닿았을때
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Item")
        {
            Items item = other.GetComponent<Items>();
            switch (item.type)
            {
                case Items.Type.Dust:
                    hasDust += item.value;
                    if (hasDust > maxDust)
                        hasDust = maxDust;
                    break;
            }
            Destroy(other.gameObject);
        }
    }
    void CheckGrounded()
    {
        RaycastHit hit;
        float raycastDistance = 0.5f;

        Vector3 raycastOrigin = transform.position + Vector3.up * 0.1f;
        anim.ResetTrigger("playFall");

        LayerMask combinedLayers = groundLayer | wallLayer; //TODO

        bool isGroundedNow = Physics.Raycast(raycastOrigin, Vector3.down, out hit, raycastDistance, combinedLayers);

        Debug.DrawRay(raycastOrigin, Vector3.down * raycastDistance, isGroundedNow ? Color.green : Color.red);

        anim.SetBool("isGrounded", true);
        

        if (!isGroundedNow)
        {
            anim.SetBool("isGrounded", false);
            PlayFall();

        }
        if (isGroundedNow && !wasGrounded)
        {
            Debug.Log("Grounded");
            anim.SetBool("isJumping", false);
            _isJumping = false;
        }
        wasGrounded = isGroundedNow;
    }
    void RotationFreeze()
    {
        playerRigidbody.angularVelocity = Vector3.zero;
    }
    //void StopBeforeObject() //충돌전 확인
    //{
    //    _isBorder = Physics.Raycast(transform.position + Vector3.up, transform.forward, 2, LayerMask.GetMask("Wall"));
    //}
    private void FixedUpdate()
    {
        PlayerMove();
        RotationFreeze();
        //StopBeforeObject();
    }
}
