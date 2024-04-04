using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RobotZone : MonoBehaviour
{
    public GameObject Robot;
    
    public List<GameObject> RobotEnemy;
    public List<GameObject> Lazer;
    public Camera mainCamera;
    public Transform cameraPos;
    public Transform socket;
    public float moveSpeed = 1.0f;
    public float rotationSpeed = 1.0f;
    public GameObject player;
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            Robot.GetComponent<Animator>().enabled = true;
            NewPlayerController.stage = 2;
            GetComponent<BoxCollider>().enabled = false;
            Debug.Log(NewPlayerController.stage);
            StartCoroutine(CameraMove());

            StartCoroutine(RobotFall());

        }
    }

    IEnumerator RobotFall()
    {
        yield return new WaitForSeconds(8f);
        for(int i = 0; i < RobotEnemy.Count; i++)
        {
            RobotEnemy[i].SetActive(true);
            
            RobotEnemy[i].GetComponent<Animator>().enabled = true;
        }
        

        yield return new WaitForSeconds(4f);
        for (int i = 0; i < RobotEnemy.Count; i++)
        {
            RobotEnemy[i].GetComponent<NavMeshAgent>().enabled = true;
            RobotEnemy[i].GetComponent<Enemy>().enabled = true;
        }
        for (int i = 0; i < Lazer.Count; i++)
        {
            Lazer[i].SetActive(true);
        }
        
    }
    IEnumerator CameraMove()
    {
        CameraMode.IsGamePause = true;
        yield return new WaitForSeconds(1.0f);
        player.transform.LookAt(Robot.transform);
        mainCamera.transform.SetParent(cameraPos);

        while (Vector3.Distance(mainCamera.transform.localPosition, Vector3.zero) > 1.5f)
        {
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, cameraPos.position, moveSpeed * Time.deltaTime);
            mainCamera.transform.rotation = Quaternion.Slerp(mainCamera.transform.rotation, cameraPos.rotation, rotationSpeed * Time.deltaTime);
            yield return null;
        }

        yield return new WaitForSeconds(10.0f);
        mainCamera.transform.SetParent(socket);
        mainCamera.transform.localPosition = Vector3.zero;
        mainCamera.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);


        CameraMode.IsGamePause = false;
    }
}
