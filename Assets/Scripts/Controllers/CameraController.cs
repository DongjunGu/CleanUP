using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;

    public Vector3 offset;
    public Vector3 originalOffset;
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    //[Range(0.01f, 1.0f)]
    public float cameraSmooth = 0.5f;

    public bool LookAtPlayer = false;

    public bool rotateAroundPlayer = true;

    public float rotationSpeed = 2.0f;
    public float minVerticalAngle = -90f; 

    void Start()
    {
        // Follow the player
        //offset = transform.position - target.position;
        //originalPosition = transform.position;
        originalRotation = transform.rotation;
        originalOffset = offset;
        Cursor.lockState = CursorLockMode.Locked;

    }
    void LateUpdate()
    {
        transform.position = target.position + offset;
        if (Input.GetKey(KeyCode.LeftAlt))
        {
            float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
            float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed;

            if (rotateAroundPlayer)
            {
                Quaternion horizontalRotation = Quaternion.AngleAxis(mouseX, Vector3.up);
                Quaternion verticalRotation = Quaternion.AngleAxis(mouseY, -Vector3.right);
                offset = horizontalRotation * verticalRotation * offset;
            }
            Vector3 newPos = target.position + offset;

            transform.position = Vector3.Slerp(transform.position, newPos, cameraSmooth);

            if (LookAtPlayer || rotateAroundPlayer)
                transform.LookAt(target);
        }
        else
        {
            // Restore the original camera position and rotation
            transform.position = target.position + originalOffset;
            transform.rotation = originalRotation;
        }
    }

}