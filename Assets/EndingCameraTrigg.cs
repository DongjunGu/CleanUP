using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndingCameraTrigg : MonoBehaviour
{
    public Camera mainCamera;
    public GameObject player;
    public Transform socket;
    public Transform cameraPos;
    public GameObject SpringArm;

    public TMPro.TMP_Text myLabel;
    public GameObject TMPImage;
    public string text;
    public int language;
    public GameObject TMPObj;

    public AudioClip clipTalk;

    private void Update()
    {
        language = LanguageToggle.mainLanguage;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            StartCoroutine(CameraMove());
        }
    }

    public IEnumerator CameraMove()
    {
        SoundController.bgmNum = 6;
        GetComponent<BoxCollider>().enabled = false;
        yield return new WaitForSeconds(1.0f);
        Animator playerAnim = player.GetComponent<Animator>();
        playerAnim.SetBool("isRun", false);
        player.GetComponent<NewPlayerController>().enabled = false;
        yield return new WaitForSeconds(0.01f);
        SpringArm.GetComponent<SpringArmCamera>().enabled = false;
        player.transform.localRotation = Quaternion.identity;
        mainCamera.transform.SetParent(cameraPos);

        while (Vector3.Distance(mainCamera.transform.localPosition, Vector3.zero) > 1.5f)
        {
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, cameraPos.position, 0.5f * Time.deltaTime);
            mainCamera.transform.rotation = Quaternion.Slerp(mainCamera.transform.rotation, cameraPos.rotation, 0.5f * Time.deltaTime);
            yield return null;
        }

        yield return new WaitForSeconds(1.0f);

        yield return StartCoroutine(CameraRotate());
        

        yield return new WaitForSeconds(1.0f);
        mainCamera.transform.SetParent(socket);
        mainCamera.transform.localPosition = Vector3.zero;
        mainCamera.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
        yield return new WaitForSeconds(1.0f);
        yield return StartCoroutine(Text());
        SpringArm.GetComponent<SpringArmCamera>().enabled = true;
        player.GetComponent<NewPlayerController>().enabled = true;

        yield return new WaitForSeconds(3.0f);

    }

    IEnumerator Text()
    {
        TMPImage.SetActive(true);
        text = TalkManager.table.datas[22].Text[language];
        int cur = 0;
        while (cur < text.Length)
        {
            myLabel.text += text[cur++];
            SoundController.Instance.PlayBossSound("Talk", clipTalk, 0.1f);
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(1.5f);
        TMPObj.GetComponent<TextMeshProUGUI>().text = "";
        TMPImage.SetActive(false);
    }

    IEnumerator CameraRotate()
    {
        Quaternion startRotation = cameraPos.transform.rotation;
        Quaternion targetRotation = Quaternion.Euler(32.45f, 26.018f, 0f);
        float elapsedTime = 0f;
        float rotationDuration = 10f;
        while (elapsedTime < rotationDuration)
        {
            cameraPos.transform.rotation = Quaternion.Slerp(startRotation, targetRotation, elapsedTime / rotationDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

    }
}
