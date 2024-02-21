//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

////[RequireComponent(typeof(CharacterController))]

//public class TestController : MonoBehaviour
//{
//    [SerializeField]
//    private float moveSpeed;
//    [SerializeField]
//    private float rotCamXAxisSpeed = 5.0f;
//    [SerializeField]
//    private float rotCamYAxisSpeed = 3.0f;
//    [SerializeField] LayerMask groundLayer = 1 << 9;
//    [SerializeField] LayerMask wallLayer = 1 << 6;
//    private float limitMinX = -80;
//    private float limitMaxX = 50;
//    private float eulerAngleX;
//    private float eulerAngleY;

//    public GameObject[] weapons;
//    public bool[] hasWeapons;
//    Animator anim;

//    bool _isJumping;
//    bool _isDodge;
//    bool _obtainItem;
//    bool _swapItem1;
//    bool _swapItem2;
//    bool _attackKey;
//    bool _isAttack;
//    bool _isAttacking = false;
//    bool _isGrounded;
//    bool _isBorder; //충돌감지
//    bool _isBorderDodge;
//    bool _canDoubleJump = true;

//    Vector3 dir;
//    Vector3 dogeVec;
//    Vector3 initialJumpPosition;
//    GameObject getItem;
//    Weapons orginWeapon;
//    private Rigidbody playerRigidbody;

//    void Awake()
//    {
//        //characterController = GetComponent<CharacterController>();
//        playerRigidbody = GetComponent<Rigidbody>();
//        anim = GetComponent<Animator>();
//        // Initialize Cursor settings
//        Cursor.visible = false;
//        Cursor.lockState = CursorLockMode.Locked;
//    }

//    void Update()
//    {
//        GetInput();
//        PlayerJump();

//        UpdateRotate();
//        UpdateMove();

//    }
//    void GetInput()
//    {
//        //_obtainItem = Input.GetButtonDown("Grab");
//        //_swapItem1 = Input.GetButtonDown("SwapItem1");
//        //_swapItem2 = Input.GetButtonDown("SwapItem2");
//        //_attackKey = Input.GetButtonDown("Attack1");

//    }
//    void PlayerJump()
//    {
//        if (Input.GetKeyDown(KeyCode.Space) && (!_isJumping || (_canDoubleJump && !_isDodge)))
//        {
//            if (_isJumping)
//            {
//                _canDoubleJump = false;
//                //anim.SetTrigger("playDoubleJump");
//                //anim.SetBool("isDoubleJumping", true);
//                //playerRigidbody.AddForce(Vector3.up * 10.0f, ForceMode.Impulse);

//                return;
//            }
//            playerRigidbody.AddForce(Vector3.up * 10.0f, ForceMode.Impulse);
//            //anim.SetBool("isJumping", true);
//            _isJumping = true;
//            //anim.SetTrigger("playJump");

//            if (_isAttacking)
//                return;


//        }
//    }

//    void UpdateRotate()
//    {
//        float mouseX = Input.GetAxis("Mouse X");
//        float mouseY = Input.GetAxis("Mouse Y");

//        eulerAngleY += mouseX * rotCamXAxisSpeed;
//        eulerAngleX -= mouseY * rotCamYAxisSpeed;

//        eulerAngleX = ClampAngle(eulerAngleX, limitMinX, limitMaxX);

//        transform.rotation = Quaternion.Euler(eulerAngleX, eulerAngleY, 0);
//    }

//    private void UpdateMove()
//    {
//        float x = Input.GetAxisRaw("Horizontal");
//        float z = Input.GetAxisRaw("Vertical");

//        Vector3 dir = new Vector3(x, 0, z).normalized;
//        transform.Translate(dir * moveSpeed * Time.deltaTime, Space.Self);
//        //Vector3 dir = transform.rotation * new Vector3(x, 0, z);

//        //Vector3 moveForce = new Vector3(dir.x * moveSpeed, 0, dir.z * moveSpeed);

//        //characterController.Move(moveForce * Time.deltaTime);
//    }

//    private float ClampAngle(float angle, float min, float max)
//    {
//        if (angle < -360) angle += 360;
//        if (angle > 360) angle -= 360;

//        return Mathf.Clamp(angle, min, max);
//    }
//}