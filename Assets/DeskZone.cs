using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DeskZone : MonoBehaviour
{
    public static bool IsDeskView = false;
    public GameObject SpringArm;
    public Camera mainCamera;
    public Transform cameraPos;
    public Transform cameraMousePos;
    public Transform socket;
    public float moveSpeed = 1.0f;
    public float rotationSpeed = 1.0f;
    public GameObject player;
    public GameObject monitorNoise;
    public GameObject Almondzone;
    public GameObject MonitorText;
    public GameObject MonitorUI;
    public GameObject MonitorAnim;
    public GameObject Mouse;
    private GameObject mouseObj;
    public UnityEngine.Events.UnityEvent NextStep;
    private void Start()
    {
        SpawnMouse();
    }

    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            NewPlayerController.stage = 3;
            
            StartCoroutine(CameraMove());
            GetComponent<Collider>().enabled = false;
        }
    }
    public void SpawnMouse()
    {
        mouseObj = Instantiate(Mouse);
    }

    public void DestroyMouse()
    {
        Destroy(mouseObj);
    }
    public IEnumerator CameraMove()
    {
        IsDeskView = true;
        Animator playerAnim = player.GetComponent<Animator>();
        playerAnim.SetBool("isRun", false);
        player.GetComponent<NewPlayerController>().enabled = false;
        yield return new WaitForSeconds(0.01f);
        SpringArm.GetComponent<SpringArmCamera>().enabled = false;
        player.transform.localRotation = Quaternion.identity;
        mainCamera.transform.SetParent(cameraPos);

        while (Vector3.Distance(mainCamera.transform.localPosition, Vector3.zero) > 1.5f)
        {
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, cameraPos.position, moveSpeed * Time.deltaTime);
            mainCamera.transform.rotation = Quaternion.Slerp(mainCamera.transform.rotation, cameraPos.rotation, rotationSpeed * Time.deltaTime);
            yield return null;
        }
        SoundController.bgmNum = 4;
        yield return new WaitForSeconds(1.0f);
        monitorNoise.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        monitorNoise.SetActive(false);
        yield return new WaitForSeconds(2.0f);
        
        MonitorUI.SetActive(true);
        MonitorAnim.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        MonitorAnim.SetActive(false);
        MonitorText.SetActive(true);
        yield return new WaitForSeconds(5.0f);

        player.GetComponent<NewPlayerController>().enabled = true;
        yield return new WaitForSeconds(3.0f);

    }

    public void CamMove()
    {
        StartCoroutine(CameraMoveToMouse());
    }
    public void CamBackToMonitor()
    {
        StartCoroutine(CameraBackToMonitor());
    }
    public IEnumerator CameraMoveToMouse()
    {
        IsDeskView = false;
        Animator playerAnim = player.GetComponent<Animator>();
        playerAnim.SetBool("isRun", false);
        player.GetComponent<NewPlayerController>().enabled = false;
        yield return new WaitForSeconds(0.01f);
        
        mainCamera.transform.SetParent(cameraMousePos);

        while (Vector3.Distance(mainCamera.transform.localPosition, Vector3.zero) > 1.5f)
        {
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, cameraMousePos.position, moveSpeed * Time.deltaTime);
            mainCamera.transform.rotation = Quaternion.Slerp(mainCamera.transform.rotation, cameraMousePos.rotation, rotationSpeed * Time.deltaTime);
            yield return null;
        }
        mouseObj.GetComponent<Animator>().enabled = true;
        mouseObj.GetComponent<MouseEnemy>().enabled = true;
        yield return new WaitForSeconds(5f);
        mouseObj.GetComponent<Animator>().enabled = false;
        yield return new WaitForSeconds(0.1f);
        mainCamera.transform.SetParent(socket);
        mainCamera.transform.localPosition = Vector3.zero;
        mainCamera.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
        SpringArm.GetComponent<SpringArmCamera>().enabled = true;
        player.GetComponent<NewPlayerController>().enabled = true;
    }
    public IEnumerator CameraBackToMonitor()
    {
        yield return new WaitForSeconds(3.0f);
        IsDeskView = true;
        Animator playerAnim = player.GetComponent<Animator>();
        playerAnim.SetBool("isRun", false);
        player.GetComponent<NewPlayerController>().enabled = false;
        yield return new WaitForSeconds(0.01f);
        SpringArm.GetComponent<SpringArmCamera>().enabled = false;
        player.transform.localRotation = Quaternion.identity;
        mainCamera.transform.SetParent(cameraPos);

        while (Vector3.Distance(mainCamera.transform.localPosition, Vector3.zero) > 1.5f)
        {
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, cameraPos.position, moveSpeed * Time.deltaTime);
            mainCamera.transform.rotation = Quaternion.Slerp(mainCamera.transform.rotation, cameraPos.rotation, rotationSpeed * Time.deltaTime);
            yield return null;
        }

        player.GetComponent<NewPlayerController>().enabled = true;
        yield return new WaitForSeconds(3.0f);
        NextStep?. Invoke();
    }
}