using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushCubeHint : MonoBehaviour
{
    public Camera mainCamera;
    public Transform PushCubeCameraPos;
    public Transform socket;
    public float moveSpeed = 1.5f;
    public float rotationSpeed = 1.5f;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            StartCoroutine(CameraMove());
            GetComponent<Collider>().enabled = false;
        }
    }
    IEnumerator CameraMove()
    {
        CameraMode.IsGamePause = true;
        yield return new WaitForSeconds(1.0f);

        mainCamera.transform.SetParent(PushCubeCameraPos);
        while (Vector3.Distance(mainCamera.transform.localPosition, Vector3.zero) > 0.5f)
        {
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, PushCubeCameraPos.position, moveSpeed * Time.deltaTime);
            mainCamera.transform.rotation = Quaternion.Lerp(mainCamera.transform.rotation, PushCubeCameraPos.rotation, rotationSpeed * Time.deltaTime);
            yield return null;
        }
        yield return new WaitForSeconds(3.0f);

        mainCamera.transform.SetParent(socket);
        mainCamera.transform.localPosition = Vector3.zero;
        mainCamera.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);

        CameraMode.IsGamePause = false;
    }
}
