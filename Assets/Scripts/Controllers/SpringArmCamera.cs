using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class SpringArmCamera : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    public Vector3 originalOffset;
    public float rotationSpeed = 10.0f;

    private float currentCameraRotationX;

    [SerializeField]
    private float lookSensitivity;
    public Camera mainCamera;
    public GameObject cameraObject;
    Vector3 _delta = new Vector3(0.0f, 0.0f, -12.0f);
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

        float dist = Vector3.Distance(mainCamera.transform.position, target.position);

        RaycastHit hit;

        Vector3 direction = (mainCamera.transform.position - target.position);

        if (Physics.Raycast(target.transform.position, direction, out hit, direction.magnitude, LayerMask.GetMask("Wall")))
        {
            mainCamera.transform.position = hit.point - direction.normalized * 0.5f;
            
        }
        else
        {
            mainCamera.transform.localPosition = Vector3.zero;
            mainCamera.transform.Translate(_delta);

        }

        //if (Physics.Raycast(mainCamera.transform.position, target.position - mainCamera.transform.position, out hit, dist, LayerMask.GetMask("Wall")))
        //{
        //    mainCamera.transform.position = hit.point;
        //    Debug.Log("WALL!!");
        //}
        //else
        //{
        //    mainCamera.transform.localPosition = Vector3.zero;
        //    mainCamera.transform.Translate(_delta);
        //    Debug.Log("ORIGIN");
        //}
    }
}