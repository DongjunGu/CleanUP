using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeButton : MonoBehaviour
{
    public GameObject player;
    public GameObject bridgeButton;
    private bool bridgeActivated = false;
    public Animator animator;
    public GameObject bridgeBlock;
    public Camera mainCamera;
    public Transform pushCameraPos;
    public Transform socket;
    public float moveSpeed = 0.5f;
    public float rotationSpeed = 0.5f;
    bool hasAllset = false;
    bool isActive = false;
    void Start()
    {

    }
    void Update()
    {
        ActiveBridge();
        StartAnimation();

        if (hasAllset)
        {
            StartCoroutine(CameraMove());
            hasAllset = false;
        }
    }

    void ActiveBridge()
    {
        if (bridgeActivated)
        {
            return;
        }

        if (Pushable.allBlockSet)
        {
            BoxCollider boxcoll = GetComponent<BoxCollider>();
            boxcoll.enabled = true;
            
            hasAllset = true;
        }
    }
    void StartAnimation()
    {
        if (isActive)
        {
            animator.SetBool("playerIn", true);
            Animator bridgeAnim = bridgeBlock.GetComponent<Animator>();
            bridgeAnim.enabled = true;
        }

    }
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            isActive = true;
        }
    }
    IEnumerator CameraMove()
    {
        CameraMode.IsGamePause = true;
        yield return new WaitForSeconds(2.0f);
        mainCamera.orthographic = false;

        mainCamera.transform.SetParent(pushCameraPos);
        while (Vector3.Distance(mainCamera.transform.localPosition, Vector3.zero) > 0.5f)
        {
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, pushCameraPos.position, moveSpeed * Time.deltaTime);
            mainCamera.transform.rotation = Quaternion.Lerp(mainCamera.transform.rotation, pushCameraPos.rotation, rotationSpeed * Time.deltaTime);
            yield return null;
        }
       
        //yield return new WaitForSeconds(1.0f);
        bridgeButton.SetActive(true);
        bridgeActivated = true;

        yield return new WaitForSeconds(4.0f);

        HpBarUI hpBar = player.GetComponent<NewPlayerController>().hpUI;
        hpBar.gameObject.SetActive(true);
        mainCamera.transform.SetParent(socket);
        mainCamera.transform.localPosition = Vector3.zero;
        mainCamera.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
        CameraMode.IsGamePause = false;
    }
}

