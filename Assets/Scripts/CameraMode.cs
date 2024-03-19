using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMode : MonoBehaviour
{
    public static bool IsGamePause = false;
    public Camera mainCamera;
    public GameObject springArm;
    public GameObject tableView;
    public GameObject player;

    private Vector3 targetPosition;
    private float transitionSpeed = 1.0f;

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
    void SwitchToTableView()
    {
        springArm.GetComponent<SpringArmCamera>().enabled = false;
        targetPosition = tableView.transform.position;
        mainCamera.transform.parent = tableView.transform;
        StartCoroutine(MoveCamera());
        //MainCamera Projection Orthographic size 21
        mainCamera.orthographic = true;
        mainCamera.orthographicSize = 21;
    }

    void ReturnToOriginalPlace()
    {
        springArm.GetComponent<SpringArmCamera>().enabled = true;
        mainCamera.orthographic = false;
        targetPosition = springArm.transform.position;
        mainCamera.transform.parent = springArm.transform;
        StartCoroutine(MoveCamera());
    }

    IEnumerator MoveCamera()
    {
        IsGamePause = true;
        while (Vector3.Distance(mainCamera.transform.position, targetPosition) > 0.01f)
        {
            mainCamera.transform.localPosition = Vector3.Lerp(mainCamera.transform.localPosition, Vector3.zero, transitionSpeed * Time.deltaTime);
            mainCamera.transform.localRotation = Quaternion.Lerp(mainCamera.transform.localRotation, Quaternion.identity, transitionSpeed * Time.deltaTime);
            yield return null;
        }
        IsGamePause = false;
    }
}
