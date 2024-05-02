using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LastStage : MonoBehaviour
{
    public TMPro.TMP_Text myLabel;
    public GameObject TMPImage;
    public string text;
    public int language;
    public GameObject TMPObj;
    public GameObject player;
    public Camera mainCamera;
    public Transform cameraPos;
    public Transform socket;
    public Transform BossDeadCameraPos;
    public float moveSpeed = 1.0f;
    public float rotationSpeed = 1.0f;
    public GameObject boss;
    public AudioClip clipTalk;
    public UnityEngine.Events.UnityEvent ActiveBoss;
    public UnityEngine.Events.UnityEvent ExitDoorActive;
    void OnEnable()
    {
        StartCoroutine(StartLastStage());
    }

    private void Update()
    {
        language = LanguageToggle.mainLanguage;
    }

    IEnumerator StartLastStage()
    {
        int index = 16;
        boss.GetComponent<Animator>().enabled = true;
        //Ä«¸Þ¶ó
        yield return new WaitForSeconds(2.0f);
        yield return StartCoroutine(CameraMove());
        SoundController.Instance.ResumeBackgroundMusic();
        SoundController.bgmNum = 5;
        yield return StartCoroutine(PrintText(index++));
        yield return StartCoroutine(ExtraText());
        yield return StartCoroutine(PrintText(index++));
        TMPImage.SetActive(false);
        yield return StartCoroutine(CameraMovePlayer());
        ActiveBoss?.Invoke();
    }
    IEnumerator PrintText(int index)
    {
        TMPImage.SetActive(true);
        text = TalkManager.table.datas[index].Text[language];
        int cur = 0;
        
        while (cur < text.Length)
        {
            myLabel.text += text[cur++];
            SoundController.Instance.PlayBossSound("Talk", clipTalk, 0.1f);
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(3.0f);
        TMPObj.GetComponent<TextMeshProUGUI>().text = "";
    }
    IEnumerator ExtraText()
    {
        TMPImage.SetActive(true);
        text = TalkManager.table.datas[25].Text[language];
        int cur = 0;

        while (cur < text.Length)
        {
            myLabel.text += text[cur++];
            SoundController.Instance.PlayBossSound("Talk", clipTalk, 0.1f);
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(3.0f);
        TMPObj.GetComponent<TextMeshProUGUI>().text = "";
    }

    public IEnumerator CameraMove()
    {
        Animator playerAnim = player.GetComponent<Animator>();
        playerAnim.SetBool("isRun", false);
        player.GetComponent<NewPlayerController>().enabled = false;
        yield return new WaitForSeconds(0.01f);
        mainCamera.transform.SetParent(cameraPos);
        

        while (Vector3.Distance(mainCamera.transform.localPosition, Vector3.zero) > 1.5f)
        {
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, cameraPos.position, moveSpeed * Time.deltaTime);
            mainCamera.transform.rotation = Quaternion.Slerp(mainCamera.transform.rotation, cameraPos.rotation, rotationSpeed * Time.deltaTime);
            yield return null;
        }
        boss.GetComponent<Animator>().SetBool("isTalking", true);
    }
    public IEnumerator CameraMovePlayer()
    {
        DeskZone.IsDeskView = false;
        boss.GetComponent<Animator>().SetBool("isTalking", false);
        mainCamera.transform.SetParent(socket);
        mainCamera.transform.localPosition = Vector3.zero;
        mainCamera.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
        player.GetComponent<NewPlayerController>().enabled = true;
        yield return null;
    }

    public IEnumerator AfterBossDeadCamera()
    {
        SoundController.bgmNum = 2;
        yield return new WaitForSeconds(3.0f);
        Animator playerAnim = player.GetComponent<Animator>();
        playerAnim.SetBool("isRun", false);
        player.GetComponent<NewPlayerController>().enabled = false;
        yield return new WaitForSeconds(0.01f);
        mainCamera.transform.SetParent(BossDeadCameraPos);


        while (Vector3.Distance(mainCamera.transform.localPosition, Vector3.zero) > 1.5f)
        {
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, BossDeadCameraPos.position, moveSpeed * Time.deltaTime);
            mainCamera.transform.rotation = Quaternion.Slerp(mainCamera.transform.rotation, BossDeadCameraPos.rotation, rotationSpeed * Time.deltaTime);
            yield return null;
        }
        ExitDoorActive?.Invoke();
        yield return new WaitForSeconds(2.0f);
        mainCamera.transform.SetParent(socket);
        mainCamera.transform.localPosition = Vector3.zero;
        mainCamera.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
        player.GetComponent<NewPlayerController>().enabled = true;
    }
    public void ActiveExitDoor()
    {
        StartCoroutine(AfterBossDeadCamera());
    }
}
