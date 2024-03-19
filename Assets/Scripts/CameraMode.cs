using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMode : MonoBehaviour
{
    public GameObject springArm;
    public GameObject tableView;
    public GameObject originalPlace;
    private bool switchToTableView = false;

    private Vector3 targetPosition;
    private float transitionSpeed = 2.0f;

    private Vector3 originalOffset = new Vector3(0f, 4f, 0f);
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (!switchToTableView)
            {
                SwitchToTableView();
            }
            else
            {
                ReturnToOriginalPlace();
            }
        }
    }
    void SwitchToTableView()
    {
        //springArm.transform.parent = tableView.transform;
        //springArm.transform.localPosition = Vector3.zero;
        //springArm.transform.localRotation = Quaternion.identity;
        targetPosition = tableView.transform.position;
        StartCoroutine(MoveCamera());
        springArm.transform.parent = tableView.transform;
        switchToTableView = true;
    }

    void ReturnToOriginalPlace()
    {
        //springArm.transform.parent = originalPlace.transform;
        //springArm.transform.localPosition = new Vector3(0, 0, -8);
        //springArm.transform.localRotation = Quaternion.identity;

        targetPosition = originalPlace.transform.position + originalOffset;
        StartCoroutine(MoveCamera());
        springArm.transform.parent = originalPlace.transform;
        springArm.transform.localPosition = originalOffset;
        //springArm.transform.localRotation = Quaternion.identity;
        switchToTableView = false;
    }

    IEnumerator MoveCamera()
    {
        while (Vector3.Distance(springArm.transform.position, targetPosition) > 0.01f)
        {
            springArm.transform.position = Vector3.Lerp(springArm.transform.position, targetPosition, transitionSpeed * Time.deltaTime);
            yield return null;
        }
    }
}
