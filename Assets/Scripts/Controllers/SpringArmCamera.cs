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
    public Transform cameraSocket;
    public Camera mainCamera;
    public GameObject cameraObject;
    Vector3 _delta = new Vector3(0.0f, 0.0f, -12.0f);

    float dist = 0.0f;  
    void Start()
    {
        dist = Mathf.Abs(cameraSocket.localPosition.z);
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;

    }
    void LateUpdate()
    {

        float _xRotation = Input.GetAxisRaw("Mouse Y");
        float _cameraRotationX = _xRotation * lookSensitivity;

        currentCameraRotationX -= _cameraRotationX;
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -15.0f, 70.0f);

        transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);

        //float dist = Vector3.Distance(mainCamera.transform.position, target.position);

        RaycastHit hit;

        float offset = 0.5f;
        float tempDist = dist;
        if (Physics.Raycast(transform.position, -transform.forward, out hit, dist + offset + 0.1f, LayerMask.GetMask("Wall")))
        {
            tempDist = hit.distance - offset - 0.5f;
            
        }

        if(!CameraMode.IsGamePause) cameraSocket.localPosition = new Vector3(0, 0, -tempDist);
    }

    public void AttachCamera(Transform cam)
    {
        cam.SetParent(cameraSocket);
    }
}