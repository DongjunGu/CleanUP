using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMode : MonoBehaviour
{
    public static bool IsGamePause = false;
    public static bool isTableView = false;
    public Camera mainCamera;
    public SpringArmCamera springArm;
    public GameObject tableView;
    public GameObject player;
    public GameObject playerDestination;
    public GameObject respawnZone;
    public GameObject respawnWall;
    private Transform targetPosition;
    private Vector3 targetForward;
    private float transitionSpeed = 1.0f;
    bool allSet = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            SwitchToTableView();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
            ReturnToOriginalPlace();
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" && !allSet)
        {
            if (Pushable.allBlockSet)
            {
                isTableView = false;
                springArm.enabled = true;
                
                respawnZone.SetActive(false);
                respawnWall.SetActive(false);
                GetComponent<Collider>().enabled = false;
                allSet = true;
            }
        }
            
    }
    void SwitchToTableView()
    {
        isTableView = true;
        springArm.enabled = false;
        player.transform.localRotation = Quaternion.identity;
        HpBarUI hpBar = player.GetComponent<NewPlayerController>().hpUI;
        hpBar.gameObject.SetActive(false);
        targetPosition = tableView.transform;
        targetForward = tableView.transform.forward;
        mainCamera.transform.parent = tableView.transform;
        StartCoroutine(MoveCamera());
        respawnZone.SetActive(true);
        respawnWall.SetActive(true);
        player.transform.position = playerDestination.transform.position;
        mainCamera.orthographic = true;
        mainCamera.orthographicSize = 35;
    }

    void ReturnToOriginalPlace()
    {
        isTableView = false;
        springArm.enabled = true;
        mainCamera.orthographic = false;
        HpBarUI hpBar = player.GetComponent<NewPlayerController>().hpUI;
        hpBar.gameObject.SetActive(true);
        respawnZone.SetActive(false);
        respawnWall.SetActive(false);
        targetPosition = springArm.cameraSocket;
        targetForward = springArm.transform.forward;
        springArm.AttachCamera(mainCamera.transform);
        StartCoroutine(MoveCamera());
    }

    IEnumerator MoveCamera()
    {
        //IsGamePause = true;
        while (Vector3.Distance(mainCamera.transform.localPosition, Vector3.zero) > 1.5f ||
            Vector3.Angle(mainCamera.transform.forward, targetForward) > 0.1f
            )
        {
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, targetPosition.position, transitionSpeed * Time.deltaTime);
            mainCamera.transform.localRotation = Quaternion.Lerp(mainCamera.transform.localRotation, Quaternion.identity, transitionSpeed * Time.deltaTime);
            yield return null;
        }
        //IsGamePause = false;
    }
}
