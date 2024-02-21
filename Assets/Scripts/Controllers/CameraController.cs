using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;

    public Vector3 offset;
    //[Range(0.01f, 1.0f)]
    public float cameraSmooth = 0.5f;

    public bool LookAtPlayer = false;

    public bool rotateAroundPlayer = true;

    public float rotationSpeed = 2.0f;

    [SerializeField]
    private float rotCamXAxisSpeed = 5.0f;
    [SerializeField]
    private float rotCamYAxisSpeed = 3.0f;

    private float limitMinX = -80;
    private float limitMaxX = 50;
    private float eulerAngleX;
    private float eulerAngleY;
    void Start()
    {
        // Follow the player
        offset = transform.position - target.position;
        Cursor.lockState = CursorLockMode.Locked;

    }
    void LateUpdate()
    {
        float mouseX = Input.GetAxis("Mouse X") * rotCamXAxisSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * rotCamYAxisSpeed;

        eulerAngleY += mouseX;
        eulerAngleX -= mouseY;
        eulerAngleX = ClampAngle(eulerAngleX, limitMinX, limitMaxX);

        if (rotateAroundPlayer)
        {
            Quaternion cameraAngle = Quaternion.AngleAxis(mouseX, Vector3.up);
            offset = cameraAngle * offset;
        }
        Vector3 newPos = target.position + offset;

        transform.rotation = Quaternion.Euler(eulerAngleX, eulerAngleY, 0);
        transform.position = Vector3.Slerp(transform.position, newPos, cameraSmooth);

        if (LookAtPlayer || rotateAroundPlayer)
            transform.LookAt(target);
    }
    private float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360) angle += 360;
        if (angle > 360) angle -= 360;

        return Mathf.Clamp(angle, min, max);
    }

    //void LateUpdate()
    //{
    //    float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
    //    float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed;

    //    if (rotateAroundPlayer)
    //    {
    //        Quaternion cameraAngle = Quaternion.AngleAxis(mouseX, Vector3.up);
    //        offset = cameraAngle * offset;
    //    }
    //    Vector3 newPos = target.position + offset;

    //    transform.position = Vector3.Slerp(transform.position, newPos, cameraSmooth);

    //    if (LookAtPlayer || rotateAroundPlayer)
    //        transform.LookAt(target);
    //}

}