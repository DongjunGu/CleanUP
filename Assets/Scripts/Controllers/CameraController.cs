using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;

    public Vector3 offset;
    public Vector3 originalOffset;
    public Vector3 tempOffset;
    public Vector3 groundOffset;
    private Quaternion originalRotation;
    //[Range(0.01f, 1.0f)]
    public float cameraSmooth = 0.5f;

    public bool LookAtPlayer = false;

    public bool rotateAroundPlayer = true;

    public float rotationSpeed = 2.0f;
    public float minVerticalAngle = -90f;
    Vector3 newPos;
    void Start()
    {

        originalRotation = transform.rotation;
        originalOffset = offset;
        tempOffset = offset;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;

    }

    void LateUpdate()
    {
        transform.position = target.position + offset;

        float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed;

        if (rotateAroundPlayer)
        {
            Quaternion horizontalRotation = Quaternion.AngleAxis(mouseX, Vector3.up);
            Quaternion verticalRotation = Quaternion.AngleAxis(mouseY, -Vector3.right);
            offset = horizontalRotation * verticalRotation * offset;

        }
        newPos = target.position + offset;

        transform.position = Vector3.Slerp(transform.position, newPos, cameraSmooth);

        if(Physics.Raycast(target.transform.position, offset.normalized, out RaycastHit hit, offset.magnitude + 0.5f, LayerMask.GetMask("Wall")))
        {
            //tempOffset = (hit.point - target.position) * 0.8f;
            //tempOffset.y += 2.0f;

            transform.position = hit.point - offset.normalized * 0.5f;
            //transform.rotation = originalRotation;

        }
        
        if(Physics.Raycast(target.transform.position, offset.normalized, out RaycastHit cameraHit, offset.magnitude + 0.5f, LayerMask.GetMask("Ground")))
        {
            //groundOffset = (cameraHit.point - transform.position)* 0.8f;

            //transform.position = target.position + groundOffset;
            //transform.rotation = originalRotation;
            //Debug.Log("HIT!!");
            transform.position = cameraHit.point - offset.normalized;
        }

        if (Input.GetKey(KeyCode.LeftAlt))
        {
            offset = originalOffset;

            transform.position = target.position + originalOffset;
            transform.rotation = originalRotation;
        }

        if (LookAtPlayer || rotateAroundPlayer)
            transform.LookAt(target);

    }
}
    //void LateUpdate()
    //{
    //        transform.position = target.position + offset;

    //    if (Input.GetKey(KeyCode.LeftAlt))
    //    {
    //        float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
    //        float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed;

    //        if (rotateAroundPlayer)
    //        {
    //            Quaternion horizontalRotation = Quaternion.AngleAxis(mouseX, Vector3.up);
    //            Quaternion verticalRotation = Quaternion.AngleAxis(mouseY, -Vector3.right);
    //            offset = horizontalRotation * verticalRotation * offset;

    //        }
    //        Vector3 newPos = target.position + offset;


    //        transform.position = Vector3.Slerp(transform.position, newPos, cameraSmooth);



    //        if (LookAtPlayer || rotateAroundPlayer)
    //            transform.LookAt(target);


    //    }
    //    else
    //    {
    //        RaycastHit hit;
    //        if (Physics.Raycast(target.transform.position, originalOffset, out hit, originalOffset.magnitude, LayerMask.GetMask("Wall")))
    //        {
    //            //float dist = (hit.point - target.transform.position).magnitude * 1.0f;
    //            //transform.position = target.position + originalOffset.normalized * dist;
    //            tempOffset = (hit.point - target.position) * 0.8f;
    //            tempOffset.y += 2.0f;
    //            transform.position = target.position + tempOffset;
    //            transform.rotation = originalRotation;

    //        }
    //        else
    //        {
    //            offset = originalOffset;

    //            transform.position = target.position + originalOffset;
    //            transform.rotation = originalRotation;
    //        }

    //    }
    //}

//}