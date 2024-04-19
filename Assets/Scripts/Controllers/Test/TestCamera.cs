using UnityEngine;

public class TestCamera : MonoBehaviour
{
    public float rotationSpeed = 2f;

    private void Update()
    {
        // 마우스 회전 입력 처리
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // Camera의 회전 적용 (수직 방향은 제한할 수 있음)
        transform.Rotate(Vector3.up * mouseX * rotationSpeed);
        transform.Rotate(Vector3.left * mouseY * rotationSpeed);

        // 수직 회전 제한
        float currentRotationX = transform.eulerAngles.x;
        if (currentRotationX > 80f && currentRotationX < 280f)
        {
            if (currentRotationX > 180f)
            {
                currentRotationX = Mathf.Clamp(currentRotationX, 280f, 360f);
            }
            else
            {
                currentRotationX = Mathf.Clamp(currentRotationX, 0f, 80f);
            }

            transform.eulerAngles = new Vector3(currentRotationX, transform.eulerAngles.y, 0f);
        }
    }
}
