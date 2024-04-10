using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenDoor : MonoBehaviour
{
    public Camera mainCamera;
    public Transform DoorCameraPos;
    public Transform socket;
    public float moveSpeed = 1.5f;
    public float rotationSpeed = 1.5f;
    public GameObject greenDoor;
    public GameObject particle1;
    public GameObject particle2;
    void Update()
    {
        if (Items.isObtain)
        {
            StartCoroutine(CameraMove());
            Items.isObtain = false;
        }
    }

    IEnumerator CameraMove()
    {
        CameraMode.IsGamePause = true;
        yield return new WaitForSeconds(1.0f);
        mainCamera.orthographic = false;

        mainCamera.transform.SetParent(DoorCameraPos);
        while (Vector3.Distance(mainCamera.transform.localPosition, Vector3.zero) > 0.5f)
        {
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, DoorCameraPos.position, moveSpeed * Time.deltaTime);
            mainCamera.transform.rotation = Quaternion.Lerp(mainCamera.transform.rotation, DoorCameraPos.rotation, rotationSpeed * Time.deltaTime);
            yield return null;
        }
        Destroy(greenDoor);
        particle1.SetActive(true);
        particle2.SetActive(true);
        yield return new WaitForSeconds(3.0f);
        

        mainCamera.transform.SetParent(socket);
        mainCamera.transform.localPosition = Vector3.zero;
        mainCamera.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);

        CameraMode.IsGamePause = false;
    }
}
