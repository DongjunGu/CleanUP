using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class NewPlayerController : MonoBehaviour
{
    public static int stage = 1;
    public static int direction;
    [SerializeField] public float _speed = 10.0f;
    [SerializeField] LayerMask groundLayer = 1 << 9;
    [SerializeField] LayerMask wallLayer = 1 << 6;
    [SerializeField] LayerMask obstacleLayer = 1 << 15;
    [SerializeField] LayerMask pushableLayer = 1 << 15;
    [SerializeField] Transform player;
    [SerializeField] public float _rotateSpeed = 5.0f;

    public Image RespawnImage;
    public Camera mainCamera;
    public GameObject[] weapons;
    public bool[] hasWeapons;
    public Transform respawn1;
    public Transform respawn2;
    public Transform respawn3;
    public GameObject remy;
    public GameObject[] dusts;
    public int hasDust;

    public int maxDust;
    public GameObject dustObject;
    public int maxHP;
    public int currentHp;
    Animator anim;
    private float v = 0.0f;
    private float h = 0.0f;
    public GameObject deskZone;
    public GameObject monitorText;
    public GameObject monitorUI;
    public UnityEngine.Events.UnityEvent act1;
    public UnityEngine.Events.UnityEvent act2;
    public UnityEngine.Events.UnityEvent act3;

    private Rigidbody playerRigidbody;


    bool _isJumping;
    bool _isDodge;
    bool _obtainItem;
    bool _swapItem1;
    bool _swapItem2;
    bool _swapItem3;
    bool _isSwap;
    bool _attackKey;
    bool _isAttack;
    bool _dustAttack;
    bool _isGrounded;
    bool _canDoubleJump = true;
    bool isJumpZone = false;
    bool canDodge;
    bool _isDamaged;
    bool wasGrounded = true;
    private bool _isWipeAnimationPlaying = false;

    Vector3 dir;
    Vector3 dogeVec;
    Vector3 playerDirection;
    GameObject getItem;
    GameObject getImage;
    Weapons orginWeapon;
    GameObject prefab;
    GameObject hpPrefab;
    public HpBarUI hpUI;

    int orginWeaponIndex = -1;
    float _attackDelay;
    float maxSlopeAngle = 80.0f;

    RaycastHit slopeHit;
    public static int Papernumber;
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
    void Start()
    {
        prefab = Resources.Load("HpbarTest") as GameObject;
        hpPrefab = MonoBehaviour.Instantiate(prefab, HpBarCanvas.Root) as GameObject;
        hpUI = hpPrefab.GetComponent<HpBarUI>();
        //hpUI.target = transform.Find("HpPos");
        hpUI.hp = currentHp;
        hpUI.maxHP = maxHP;
        //hpPrefab.SetActive(false);
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
        if (currentHp <= 0)
        {
            StartCoroutine(Respawn());
        }
    }
    void GetInput()
    {
        v = Input.GetAxisRaw("Vertical");
        h = Input.GetAxisRaw("Horizontal");
        _obtainItem = Input.GetButtonDown("Grab");
        _swapItem1 = Input.GetButtonDown("SwapItem1");
        _swapItem2 = Input.GetButtonDown("SwapItem2");
        _swapItem3 = Input.GetButtonDown("SwapItem3");
        _attackKey = Input.GetButtonDown("Attack1");
        _dustAttack = Input.GetButtonDown("Attack2");

    }
    void PlayerMove()
    {
        if (CameraMode.IsGamePause)
        {
            anim.SetBool("isRun", false);
            return;
        }

        bool isOnSlope = IsOnSlope();

        dir = new Vector3(h, 0, v).normalized;

        if (_isAttack)
            dir = new Vector3(h, 0, v).normalized;

        if (!(Mathf.Approximately(v, 0.0f) && Mathf.Approximately(h, 0.0f)))
        {
            Vector3 _moveHorizontal = transform.right * h;
            Vector3 _moveVertical = transform.forward * v;
            float dist = _speed * Time.deltaTime;
            anim.SetBool("isRun", true);

            playerDirection = (_moveHorizontal + _moveVertical).normalized;

            Vector3 slopeVelocity = AdjustDirectionToSlope(playerDirection);


            float offset = 1.0f;
            Vector3 raycastOrigin = transform.position + Vector3.up * 0.5f;
            if (Physics.Raycast(new Ray(raycastOrigin - slopeVelocity * offset, slopeVelocity), out RaycastHit hit, dist + offset * 2.0f, LayerMask.GetMask("Wall")))
            {
                dist = hit.distance - offset * 2.0f;
            }

            if (Physics.Raycast(new Ray(raycastOrigin - slopeVelocity * offset, slopeVelocity), out RaycastHit pushhit, dist + offset * 1.0f, LayerMask.GetMask("Pushable")))
            {
                dist = pushhit.distance - offset * 1.0f;
            }
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
            if (!(DeskZone.IsDeskView))
            {
                float mouseX = Input.GetAxis("Mouse X");
                transform.Rotate(Vector3.up * mouseX * _rotateSpeed);
            }

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
                    Invoke("FallAfterJump", 0.3f);
                    return;

                }
            }
            else
            {
                playerRigidbody.velocity = Vector3.zero;
                playerRigidbody.AddForce(Vector3.up * 15.0f, ForceMode.Impulse);
                Debug.Log("JUMP");
                anim.SetBool("isJumping", true);
                _isJumping = true;
                anim.SetTrigger("playJump");
                _canDoubleJump = true;
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
        if (Input.GetKeyDown(KeyCode.LeftShift) && !_isSwap && !_isDodge && dir != Vector3.zero && !_isJumping && !canDodge)
        {
            anim.SetTrigger("playDodge");
            _isDodge = true;
            _isJumping = true;
            canDodge = false;
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
        if (_swapItem3 && (!hasWeapons[2] || orginWeaponIndex == 2))
            return;
        int weaponIndex = -1;
        if (_swapItem1) weaponIndex = 0;
        if (_swapItem2) weaponIndex = 1;
        if (_swapItem3) weaponIndex = 2;

        if ((_swapItem1 || _swapItem2 || _swapItem3) && !_isJumping && !_isDodge)
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

                GameObject instantDust = Instantiate(dustObject, player.transform.position + Vector3.up * 2.0f, player.transform.rotation);
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
            transform.Translate(player.rotation * temp, Space.World);
        }
        player.rotation *= anim.deltaRotation;
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
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
            //anim.SetBool("isJumping", true);
            _isJumping = false;
            _canDoubleJump = false;
            playerRigidbody.AddForce(Vector3.up * 30.0f, ForceMode.Impulse);
            Debug.Log("CumpulsionJump");
        }
        if (collision.gameObject.tag == "Almond")
        {
            if (hpUI != null)
            {
                currentHp -= 30;
                hpUI.takeDamage(30);
            }
        }
        if (collision.gameObject.tag == "Mouse")
        {
            if (hpUI != null)
            {
                if (!_isDamaged)
                {
                    MouseEnemy mouseEnemy = collision.gameObject.GetComponent<MouseEnemy>();
                    currentHp -= mouseEnemy.damage;
                    hpUI.takeDamage(mouseEnemy.damage);
                    StartCoroutine(OnDamage());
                }

            }

        }


    }
    Coroutine checkInput;
    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Weapon")
            getItem = other.gameObject;

        if (other.tag == "pushable")
        {
            Vector3 dire = (other.transform.position - player.position).normalized;
            Vector3 cubeAngle = other.transform.forward;
            Vector3 playerAngle = player.position - other.transform.position;
            playerAngle.y = 0;
            playerAngle.Normalize();

            float crossProduct = Vector3.Cross(cubeAngle, playerAngle).y;
            float angle = Vector3.Angle(cubeAngle, playerAngle);

            if (crossProduct < 0)
                angle *= -1;

            if (angle > -45.0f && angle <= 45.0f)
            {
                direction = 1; //12시
            }

            if (angle > 45 && angle <= 135)
            {
                direction = 2; //3시
            }


            if ((angle > 135 && angle <= 180) || (angle <= -135 && angle > -180))
            {
                direction = 3; //6시
            }


            if (angle <= -45 && angle > -135)
            {
                direction = 4; //9시
            }



            myState = State.TriggerBox;
            if (checkInput != null) StopCoroutine(checkInput);
            checkInput = StartCoroutine(CheckingInput(angle, other));


        }
    }

    IEnumerator CheckingInput(float angle, Collider other)
    {
        while (true)
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
            if (checkInput != null)
                StopCoroutine(checkInput);
        }
        if (other.gameObject.name == "Paper1box")
            Papernumber = 0;
        if (other.gameObject.name == "Paper2box")
            Papernumber = 0;
        if (other.gameObject.name == "Paper3box")
            Papernumber = 0;
        if (other.gameObject.name == "Paper4box")
            Papernumber = 0;
        if (other.gameObject.name == "Paper5box")
            Papernumber = 0;
        if (other.gameObject.name == "Paper6box")
            Papernumber = 0;
        if (other.gameObject.name == "Paper7box")
            Papernumber = 0;
        if (other.gameObject.name == "Paper8box")
            Papernumber = 0;
        if (other.gameObject.name == "Paper9box")
            Papernumber = 0;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Item")
        {
            Debug.Log(other.gameObject.name);
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
        if (other.tag == "Enemy")
        {
            if (hpUI != null)
            {
                if (!_isDamaged)
                {
                    Enemy enemy = other.GetComponent<Enemy>();

                    currentHp -= enemy.damage;
                    hpUI.takeDamage(enemy.damage);
                    StartCoroutine(OnDamage());
                }
            }
        }
        if (other.gameObject.name == "Paper1box")
            Papernumber = 1;
        if (other.gameObject.name == "Paper2box")
            Papernumber = 2;
        if (other.gameObject.name == "Paper3box")
            Papernumber = 3;
        if (other.gameObject.name == "Paper4box")
            Papernumber = 4;
        if (other.gameObject.name == "Paper5box")
            Papernumber = 5;
        if (other.gameObject.name == "Paper6box")
            Papernumber = 6;
        if (other.gameObject.name == "Paper7box")
            Papernumber = 7;
        if (other.gameObject.name == "Paper8box")
            Papernumber = 8;
        if (other.gameObject.name == "Paper9box")
            Papernumber = 9;
    }
    public void GetLaserDamaer(int laserDamage)
    {
        currentHp -= laserDamage;
        hpUI.takeDamage(laserDamage);
    }
    IEnumerator OnDamage()
    {
        _isDamaged = true;
        yield return new WaitForSeconds(1.0f);
        _isDamaged = false;
    }
    IEnumerator OnDamageMouse()
    {
        _isDamaged = true;
        yield return new WaitForSeconds(3.0f);
        _isDamaged = false;
    }
    void CheckGrounded()
    {
        RaycastHit hit;
        float raycastDistance = 0.5f;

        Vector3 raycastOrigin = transform.position + Vector3.up * 0.1f;
        anim.ResetTrigger("playFall");

        LayerMask combinedLayers = groundLayer | wallLayer | obstacleLayer; //TODO

        bool isGroundedNow = Physics.Raycast(raycastOrigin, Vector3.down, out hit, raycastDistance, combinedLayers);

        Debug.DrawRay(raycastOrigin, Vector3.down * raycastDistance, isGroundedNow ? Color.green : Color.red);

        anim.SetBool("isGrounded", true);


        if (!isGroundedNow)
        {
            anim.SetBool("isGrounded", false);
            PlayFall();
            canDodge = true;
        }
        if (isGroundedNow && !wasGrounded)
        {
            Debug.Log("Grounded");
            anim.SetBool("isJumping", false);
            _isJumping = false;
            canDodge = false;
        }
        wasGrounded = isGroundedNow;
    }
    void RotationFreeze()
    {
        playerRigidbody.angularVelocity = Vector3.zero;
    }
    private void FixedUpdate()
    {
        PlayerMove();
        RotationFreeze();
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

        if (Vector3.Dot(player.transform.right, tempDir.normalized) < 0)
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
        if (checkInput != null)
        {
            StopCoroutine(checkInput);
        }
    }

    IEnumerator Respawn()
    {
        if (stage == 1) //Chess
        {
            bool wasPlayerMoveEnabled = enabled;
            enabled = false;

            GameObject[] enemyDusts = GameObject.FindGameObjectsWithTag("Item");
            if (enemyDusts != null)
            {
                foreach (GameObject enemyDust in enemyDusts)
                {
                    if (enemyDust.name == "item_dust(Clone)")
                        Destroy(enemyDust);
                }
            }

            anim.SetBool("isRun", false);
            remy.SetActive(false);
            RespawnImage.GetComponent<Image>().enabled = true;
            RespawnImage.GetComponent<Animator>().enabled = true;
            hpPrefab.SetActive(false);

            yield return new WaitForSeconds(1.0f);
            ChessController.DestroyEnemy();
            remy.SetActive(true);
            ChessController.SpawnEnemy();

            transform.position = respawn1.position;

            currentHp = 200;
            hpUI.hp = currentHp;

            yield return new WaitForSeconds(2.0f);

            RespawnImage.GetComponent<Image>().enabled = false;
            RespawnImage.GetComponent<Animator>().enabled = false;
            hpPrefab.SetActive(true);
            enabled = wasPlayerMoveEnabled;
        }
        if(stage == 2)
        {
            bool wasPlayerMoveEnabled = enabled;
            enabled = false;
            anim.SetBool("isRun", false);
            remy.SetActive(false);
            RespawnImage.GetComponent<Image>().enabled = true;
            RespawnImage.GetComponent<Animator>().enabled = true;
            hpPrefab.SetActive(false);

            yield return new WaitForSeconds(1.0f);
            RobotZone.Instance.DestroyEnemy();
            remy.SetActive(true);
            transform.position = respawn2.position;
            currentHp = 200;
            hpUI.hp = currentHp;

            yield return new WaitForSeconds(2.0f);

            RespawnImage.GetComponent<Image>().enabled = false;
            RespawnImage.GetComponent<Animator>().enabled = false;
            hpPrefab.SetActive(true);
            enabled = wasPlayerMoveEnabled;
        }

        if (stage == 3)
        {
            monitorUI.SetActive(false);
            monitorText.SetActive(false);
            act1?.Invoke();
            bool wasPlayerMoveEnabled = enabled;
            enabled = false;
            anim.SetBool("isRun", false);
            remy.SetActive(false);
            RespawnImage.GetComponent<Image>().enabled = true;
            RespawnImage.GetComponent<Animator>().enabled = true;
            hpPrefab.SetActive(false);
            act2?.Invoke();//mouse destroy

            yield return new WaitForSeconds(1.0f);
            remy.SetActive(true);
            act3?.Invoke(); //mouse respawn
            transform.position = respawn3.position;
            currentHp = 200;
            hpUI.hp = currentHp;
            
            yield return new WaitForSeconds(2.0f);

            RespawnImage.GetComponent<Image>().enabled = false;
            RespawnImage.GetComponent<Animator>().enabled = false;
            hpPrefab.SetActive(true);
            
            deskZone.GetComponent<Collider>().enabled = true;
            enabled = wasPlayerMoveEnabled;
        }
    }
}
