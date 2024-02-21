using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFPS : MonoBehaviour
{
    private Camera rotateToMouse; // 마우스 이동으로 카메라 회전
    private PlayerMoveFPS movement;
    private void Awake()
    {
        // 마우스 커서를 보이지 않게 설정하고, 현재 위치에 고정시킨다.
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
