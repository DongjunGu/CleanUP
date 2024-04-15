using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Cinemachine.CinemachineFreeLook;

public class DeskZone : MonoBehaviour
{
    public GameObject SpringArm;
    public Camera mainCamera;
    public Transform cameraPos;
    public Transform socket;
    public float moveSpeed = 1.0f;
    public float rotationSpeed = 1.0f;
    public GameObject player;
    public GameObject monitorNoise;
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            StartCoroutine(CameraMove());
            GetComponent<Collider>().enabled = false;
        }
    }
    public IEnumerator CameraMove()
    {
        player.GetComponent<NewPlayerController>().enabled = false;
        Animator playerAnim = player.GetComponent<Animator>();
        playerAnim.SetBool("isRun", false);
        mainCamera.transform.SetParent(cameraPos);

        while (Vector3.Distance(mainCamera.transform.localPosition, Vector3.zero) > 1.5f)
        {
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, cameraPos.position, moveSpeed * Time.deltaTime);
            mainCamera.transform.rotation = Quaternion.Slerp(mainCamera.transform.rotation, cameraPos.rotation, rotationSpeed * Time.deltaTime);
            yield return null;
        }
        yield return new WaitForSeconds(1.0f);
        monitorNoise.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        monitorNoise.SetActive(false);
        yield return new WaitForSeconds(3.0f);
        SpringArm.transform.localPosition = new Vector3(0f, 11f, -12f);
        mainCamera.transform.SetParent(socket);
        mainCamera.transform.localPosition = Vector3.zero;
        mainCamera.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
        
        player.GetComponent<NewPlayerController>().enabled = true;
    }
}
