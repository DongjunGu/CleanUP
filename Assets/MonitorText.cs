using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.PlayerSettings.Switch;
using UnityEngine.Events;

public class MonitorText : MonoBehaviour
{
    public TMPro.TMP_Text monitorText;
    public string text;
    public int language;
    public GameObject Almondzone;
    public GameObject MonitorAnim;
    public GameObject LaserEnemyObj;
    public Transform Spawnpoint;
    public GameObject LampLight;
    public GameObject player;
    public GameObject SpinImage;
    public GameObject Monitor;
    public Transform TeleportPos;
    public Camera mainCamera;
    public Transform socket;
    public GameObject SpringArm;
    public UnityEngine.Events.UnityEvent QuizController;


    void OnEnable()
    {
        monitorText.text = "";
        //StartCoroutine(MonitorTextStart());
        LastStage();
    }

    IEnumerator MonitorTextStart()
    {
        Animator monitorAnim = MonitorAnim.GetComponent<Animator>();
        int index = 4;
        yield return StartCoroutine(WelcomeText());
        yield return StartCoroutine(PrintText(index++)); //4
        Almondzone.SetActive(true);
        yield return StartCoroutine(CountNumber(15));
        DestroySpawnedObjects();
        yield return StartCoroutine(PrintText(index++)); //5
        ClearText();
        MonitorAnim.SetActive(true);
        monitorAnim.SetBool("isAngry", true);
        yield return StartCoroutine(SpawnLaserEnemy());
        MonitorAnim.SetActive(false);
        Almondzone.SetActive(true);
        yield return StartCoroutine(CountNumber(45)); //45sec
        ClearText();
        DestroySpawnedObjects();
        yield return new WaitForSeconds(2.0f);
        StartCoroutine(QuizStart());
        ClearText();
        yield return null;
    }
    public void StartQuiz()
    {
        StartCoroutine(QuizStart());
    }
    IEnumerator QuizStart()
    {
        LampLight.SetActive(true);
        int playerHp = player.GetComponent<NewPlayerController>().currentHp = 200;
        player.GetComponent<NewPlayerController>().hpUI.hp = playerHp;
        yield return new WaitForSeconds(2.0f);
        LampLight.SetActive(false);
        int index = 9;
        yield return StartCoroutine(PrintText(index++));
        QuizController?.Invoke();
    }

    void LastStage()
    {
        StartCoroutine(LastStageStart());
    }
    IEnumerator LastStageStart()
    {
        LampLight.SetActive(true);
        int playerHp = player.GetComponent<NewPlayerController>().currentHp = 200;
        player.GetComponent<NewPlayerController>().hpUI.hp = playerHp;
        yield return new WaitForSeconds(2.0f);
        LampLight.SetActive(false);
        int index = 14;
        yield return StartCoroutine(PrintText(index++)); //14
        yield return StartCoroutine(PrintText(index++)); //15
        ClearText();
        //회오리 이미지
        SpinImage.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        //화면전환
        mainCamera.transform.SetParent(socket);
        mainCamera.transform.localPosition = Vector3.zero;
        mainCamera.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
        SpringArm.GetComponent<SpringArmCamera>().enabled = true;
        //모니터 collider 비활성화
        Monitor.GetComponent<MeshCollider>().enabled = false;
        //이동스크립트 비활성화
        player.GetComponent<NewPlayerController>().enabled = false;
        player.GetComponent<Rigidbody>().useGravity = false;
        //상승
        float elapsedTime = 0.0f;
        float duration = 2.0f;

        elapsedTime += Time.deltaTime;
        while (elapsedTime < duration)
        {
            player.transform.position += Vector3.up * 10.0f * Time.deltaTime;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        player.GetComponent<Animator>().SetBool("isSpin", true);
        //모니터로 이동
        float dist = Vector3.Distance(TeleportPos.position, player.transform.position);
        while (!TelePort.teleported)
        {
            Vector3 dir = (TeleportPos.position - player.transform.position).normalized;
            player.transform.position += dir * 20.0f * Time.deltaTime;
            yield return null;
        }
        player.GetComponent<Rigidbody>().useGravity = true;

    }
    void ClearText()
    {
        monitorText.text = "";
    }
    IEnumerator WelcomeText()
    {
        text = TalkManager.table.datas[3].Text[language];
        int cur = 0;

        while (cur < text.Length)
        {
            monitorText.text += text[cur++];
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(1.5f);
    }
    IEnumerator PrintText(int index)
    {
        ClearText();
        text = TalkManager.table.datas[index].Text[language];
        int cur = 0;

        while (cur < text.Length)
        {
            monitorText.text += text[cur++];
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(2.0f);

    }

    IEnumerator CountNumber(int count)
    {
        ClearText();
        while (count >= 0)
        {
            monitorText.text = count.ToString();
            count--;
            yield return new WaitForSeconds(1f);
        }
        yield return new WaitForSeconds(2.0f);
    }

    public void DestroySpawnedObjects()
    {
        GameObject[] almondDropPoints = GameObject.FindGameObjectsWithTag("AlmondDropPos");
        foreach (GameObject obj in almondDropPoints)
        {
            Destroy(obj);
        }

        GameObject[] almonds = GameObject.FindGameObjectsWithTag("Almond");
        foreach (GameObject obj in almonds)
        {
            Destroy(obj);
        }
        GameObject laserEnemyObj = GameObject.FindGameObjectWithTag("RobotEnemy");
        Destroy(laserEnemyObj);
        Almondzone.SetActive(false);
    }
    IEnumerator SpawnLaserEnemy()
    {
        GameObject instantiatedObject = Instantiate(LaserEnemyObj, Spawnpoint.transform.position, Quaternion.identity);
        LaserEnemy laserEnemyScript = instantiatedObject.GetComponent<LaserEnemy>();
        yield return new WaitForSeconds(3.0f);

        if (laserEnemyScript != null)
        {
            laserEnemyScript.enabled = true;
        }

        while (!(LaserEnemy.IsArrived))
        {
            yield return new WaitForSeconds(1f);
        }
    }
}
