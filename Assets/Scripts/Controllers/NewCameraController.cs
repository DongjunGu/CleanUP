using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class NewCameraController : MonoBehaviour
{
    public Vector3 offset;
    public Vector3 originalOffset;
    public Transform target;
    private Quaternion originalRotation;

    void Start()
    {
        originalOffset = offset;
        originalRotation = transform.rotation;
    }

    void LateUpdate()
    {

        if (Physics.Raycast(target.transform.position + Vector3.up * 2.0f, offset.normalized, out RaycastHit hit, offset.magnitude + 0.5f, LayerMask.GetMask("Wall")))
        {
            transform.position = hit.point - offset.normalized * 0.5f;
        }
    }

}
