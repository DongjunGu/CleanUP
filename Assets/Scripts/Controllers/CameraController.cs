using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;

    public Vector3 offset;
    [Range(0.01f, 1.0f)]
    public float cameraSmooth = 0.5f;

    public bool LookAtPlayer = false;

    public bool rotateAroundPlayer = true;

    public float rotationSpeed = 2.0f;
    void Start()
    {
        // Follow the player
        offset = transform.position - target.position;
        Cursor.lockState = CursorLockMode.Locked;

    }

    void LateUpdate()
    {
        float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed;

        if (rotateAroundPlayer)
        {
            Quaternion cameraAngle = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * rotationSpeed, Vector3.up);
            offset = cameraAngle * offset;
        }
        Vector3 newPos = target.position + offset;

        transform.position = Vector3.Slerp(transform.position, newPos, cameraSmooth);

        if (LookAtPlayer || rotateAroundPlayer)
            transform.LookAt(target);
    }

}