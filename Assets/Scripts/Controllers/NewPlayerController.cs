using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class NewPlayerController : MonoBehaviour
{
    [SerializeField] public float _speed = 10.0f;
    [SerializeField] LayerMask groundLayer = 1 << 9;
    [SerializeField] LayerMask wallLayer = 1 << 6;
    [SerializeField] LayerMask obstacleLayer = 1 << 15;
    [SerializeField] LayerMask pushableLayer = 1 << 15;
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
    Vector3 playerDirection;
    GameObject getItem;
    GameObject getImage;
    Weapons orginWeapon;

    int orginWeaponIndex = -1;
    float _attackDelay;
    float maxSlopeAngle = 80.0f;

    RaycastHit slopeHit;

    public enum State
    {
        Normal, TriggerBox
    }

    State myState = State.Normal;

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
        if (CameraMode.IsGamePause) return;
        bool isOnSlope = IsOnSlope();

        dir = new Vector3(h, 0, v).normalized;

        if (_isAttack)
            dir = new Vector3(h, 0, v).normalized;
        
        if (!(v == 0 && h == 0))
        {
            Vector3 _moveHorizontal = transform.right * h;
            Vector3 _moveVertical = transform.forward * v;
            float dist = _speed * Time.deltaTime;
            anim.SetBool("isRun", true);

            playerDirection = (_moveHorizontal + _moveVertical).normalized;

            Vector3 slopeVelocity = AdjustDirectionToSlope(playerDirection);

            
            float offset = 1.0f;
            if (Physics.Raycast(new Ray(transform.position - slopeVelocity * offset, slopeVelocity), out RaycastHit hit, dist + offset * 2.0f, LayerMask.GetMask("Wall")))
            {
                dist = hit.distance - offset * 2.0f;
            }

            if (Physics.Raycast(new Ray(transform.position - slopeVelocity * offset, slopeVelocity), out RaycastHit pushhit, dist + offset * 1.0f, LayerMask.GetMask("Pushable")))
            {
                dist = pushhit.distance - offset * 1.0f;
            }
            //else
            //{
            //    anim.SetBool("isPush", false);
            //}
            transform.Translate(slopeVelocity * dist, Space.World);

            Quaternion targetRotation = Quaternion.LookRotation(new Vector3(slopeVelocity.x, 0, slopeVelocity.z), Vector3.up);
            player.rotation = Quaternion.Slerp(player.rotation, targetRotation, Time.deltaTime * _rotateSpeed);
        }
        else
        {
            anim.SetBool("isRun", false);
        }

        if (!(CameraMode.isTableView))
        {
            float mouseX = Input.GetAxis("Mouse X");
            transform.Rotate(Vector3.up * mouseX * _rotateSpeed);
        }
        
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
                    playerRigidbody.AddForce(Vector3.up * 15.0f, ForceMode.Impulse);
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



                Vector3 nextVec = hit.point - player.transform.position;
                nextVec.y = 15;

                float lookAngle = Vector3.Angle(player.transform.forward, nextVec.normalized); // 사이각구하기

                if (Vector3.Dot(player.transform.right, nextVec.normalized) < 0) // 내적을 계산해서 0 : 직각, 음수 : 반시계
                {
                    player.transform.Rotate(Vector3.up * -lookAngle); //반시계
                }
                else
                {
                    player.transform.Rotate(Vector3.up * lookAngle); //시계
                }

                GameObject instantDust = Instantiate(dustObject, player.transform.position, player.transform.rotation);
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
                //Destroy(getItem);

                //if (getItem.name == "Broom")
                //{
                //    weaponImage1.SetActive(true);

                //}
                //else if (getItem.name == "Hammer")
                //{
                //    weaponImage2.SetActive(true);
                //}

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
            anim.SetBool("isJumping", true);
            _isJumping = true;
            playerRigidbody.AddForce(Vector3.up * 30.0f, ForceMode.Impulse);
            Debug.Log("CumpulsionJump");
            Debug.Log(collision.gameObject.tag);
        }

        if (((1 << collision.gameObject.layer) & obstacleLayer) != 0) //Layer
        {
            
        }


    }
    Coroutine checkInput;
    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Weapon")
            getItem = other.gameObject;

        if(other.tag == "pushable")
        {
            Vector3 dire = (other.transform.position - player.position).normalized;
            //Debug.DrawRay(player.position, player.transform.forward * 10f, Color.red);
            //Debug.DrawRay(player.position, dire * 10f, Color.green);
            Vector3 cubeAngle = other.transform.forward;
            Vector3 playerAngle = player.position - other.transform.position;
            playerAngle.y = 0;
            playerAngle.Normalize();

            float crossProduct = Vector3.Cross(cubeAngle, playerAngle).y;
            float angle = Vector3.Angle(cubeAngle, playerAngle);
            
            if (crossProduct < 0)
                angle *= -1;

            myState = State.TriggerBox;
            if(checkInput != null) StopCoroutine(checkInput);
            checkInput = StartCoroutine(CheckingInput(angle, other));

            
        }
    }

    IEnumerator CheckingInput(float angle, Collider other)
    {
        while(true) 
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                StopAllCoroutines();
                if (angle > -45.0f && angle <= 45.0f)
                {
                    StartCoroutine(MovingToPos(other.gameObject, other.transform.position + Vector3.forward * 6.0f));
                }

                if (angle > 45 && angle <= 135)
                {
                    StartCoroutine(MovingToPos(other.gameObject, other.transform.position + Vector3.right * 6.0f));
                }


                if ((angle > 135 && angle <= 180) || (angle <= -135 && angle > -180))
                {
                    StartCoroutine(MovingToPos(other.gameObject, other.transform.position + Vector3.back * 6.0f));
                }


                if (angle <= -45 && angle > -135)
                {
                    StartCoroutine(MovingToPos(other.gameObject, other.transform.position + Vector3.left * 6.0f));
                }

                GetComponent<NewPlayerController>().enabled = false;
                transform.SetParent(other.transform);
            }
            yield return null;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Weapon")
            getItem = null;

        if (other.tag == "pushable")
        {
            if(checkInput != null) 
                StopCoroutine(checkInput);
        }

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

        LayerMask combinedLayers = groundLayer | wallLayer  | obstacleLayer; //TODO

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

    public bool IsOnSlope()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        if (Physics.Raycast(ray, out slopeHit, 2.0f, obstacleLayer))
        {
            var angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle != 0f && angle < maxSlopeAngle;
        }
        return false;
    }
    Vector3 AdjustDirectionToSlope(Vector3 direction)
    {
        return Vector3.ProjectOnPlane(direction, slopeHit.normal);
    }
    //private void OnControllerColliderHit(ControllerColliderHit hit)
    //{
    //    if(hit.transform.tag == "pushable")
    //    {
    //        Rigidbody box = hit.collider.GetComponent<Rigidbody>();

    //        if(box != null)
    //        {
    //            Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, 0);
    //            box.velocity = pushDir * _pushForce;
    //        }
    //    }
    //}
    IEnumerator MovingToPos(GameObject otherObject, Vector3 pos)
    {
        Vector3 dir = pos - transform.position;
        float dist = dir.magnitude;
        dir.Normalize();
        while (dist > 0.0f)
        {
            float delta = Time.deltaTime * 5.0f;
            if (dist < delta)
            {
                delta = dist;
            }
            dist -= delta;
            anim.SetBool("isRun", true);
            transform.Translate(dir * delta, Space.World);
            yield return null;
        }
        anim.SetBool("isRun", false);
        Vector3 tempDir = otherObject.transform.position - player.transform.position;
        float rotateAngle = Vector3.Angle(player.transform.forward, tempDir.normalized);
        
        if(Vector3.Dot(player.transform.right, tempDir.normalized) < 0)
        {
            player.transform.Rotate(Vector3.up * -rotateAngle);
            //transform.Rotate(Vector3.up * -rotateAngle);
        }
        else
        {
            player.transform.Rotate(Vector3.up * rotateAngle);
            //transform.Rotate(Vector3.up * rotateAngle);
        }
    }

    public void StopChecking()
    {
        if(checkInput !=  null)
        {
            StopCoroutine(checkInput);
        }
    }
}
