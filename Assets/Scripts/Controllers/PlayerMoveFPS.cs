using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMoveFPS : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed;
    private Vector3 moveForce;

    private CharacterController characterController;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        characterController.Move(moveForce * Time.deltaTime);
    }

    public void MoveTo(Vector3 dir)
    {
        dir = transform.rotation * new Vector3(dir.x, 0, dir.z);

        moveForce = new Vector3(dir.x * moveSpeed, moveForce.y, dir.z * moveSpeed); 
    }
}
