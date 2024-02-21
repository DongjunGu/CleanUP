using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFPS : MonoBehaviour
{
    private Camera rotateToMouse; // ���콺 �̵����� ī�޶� ȸ��
    private PlayerMoveFPS movement;
    private void Awake()
    {
        // ���콺 Ŀ���� ������ �ʰ� �����ϰ�, ���� ��ġ�� ������Ų��.
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        rotateToMouse = GetComponent<Camera>();
        movement = GetComponent<PlayerMoveFPS>();
    }

    private void Update()
    {
        UpdateRotate();
        UpdateMove();
    }

    private void UpdateRotate()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        rotateToMouse.UpdateRotate(mouseX, mouseY);
    }

    void UpdateMove()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        movement.MoveTo(new Vector3(x, 0, z));
    }
}
