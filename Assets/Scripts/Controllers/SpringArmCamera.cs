using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class SpringArmCamera : MonoBehaviour
{
    public Transform target;

    public Vector3 offset;
    public Vector3 originalOffset;

    public bool LookAtPlayer = false;
    public bool rotateAroundPlayer = true;

    public float rotationSpeed = 2.0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }
    void LateUpdate()
    {
        transform.Rotate(-Input.GetAxis("Mouse Y") * rotationSpeed, 0f, 0f);
    }
}
