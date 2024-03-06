using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class SpringArmCamera : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    public Vector3 originalOffset;
    Vector3 newPos;
    public float rotationSpeed = 10.0f;
    public bool LookAtPlayer = false;
    public bool rotateAroundPlayer = true;

    [SerializeField]
    private float cameraRotationLimit;
    private float currentCameraRotationX;

    [SerializeField]
    private float lookSensitivity;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }
    void LateUpdate()
    {
        float _xRotation = Input.GetAxisRaw("Mouse Y");
        float _cameraRotationX = _xRotation * lookSensitivity;

        currentCameraRotationX -= _cameraRotationX;
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -20.0f, 70.0f);

        transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);

    }
}
