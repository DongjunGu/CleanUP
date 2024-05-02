using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using static UnityEditor.SceneView;

public class RobotZone : MonoBehaviour
{
    public GameObject Robot;

    public List<GameObject> RobotEnemy;
    public List<Transform> RobotEnemySpawnPos;
    public GameObject Calendar;
    private static RobotZone instance;
    private List<GameObject> instantiatedObjects = new List<GameObject>();

    public TMPro.TMP_Text myLabel;
    public GameObject TMPImage;
    public string text;
    public int language; 
    public GameObject TMPObj;
    public List<GameObject> Laser = new List<GameObject>();
    public Camera mainCamera;
    public Transform cameraPos;
    public Transform socket;
    public float moveSpeed = 1.0f;
    public float rotationSpeed = 1.0f;
    public GameObject player;
    public GameObject laser3;
    public AudioClip clipRobotStand;
    public AudioClip clipRobotVoiceFull;
    public AudioClip clipRobotVoiceShort;
    public AudioClip clipRobotEnemyDrop;
    public AudioClip clipRobotDown;
    public AudioClip clipRobotPointing;
    GameObject mainRobot;
    GameObject Robotenemies;
    
    public static bool hasClearedRobot = false;
    
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        mainRobot = Instantiate(instance.Robot);
        mainRobot.GetComponent<Animator>().enabled = false;

    }

    void Update()
    {
        language = LanguageToggle.mainLanguage;
        ClearRobotStage();
    }
   
    public static RobotZone Instance
    {
        get
        {
            if (instance == null)
                Debug.Log("NO INSTANCE");
            return instance;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            mainRobot.GetComponent<Animator>().enabled = true;
            NewPlayerController.stage = 2;
            SoundController.bgmNum = 3;
            GetComponent<BoxCollider>().enabled = false;
            StartCoroutine(CameraMove());
            StartCoroutine(LaserActive());
        }
    }
    public void StartRobot()
    {
        GetComponent<BoxCollider>().enabled = true;
        mainRobot = Instantiate(instance.Robot);
        mainRobot.GetComponent<Animator>().enabled = false;
    }
    IEnumerator SpawnEnemy()
    {
        GameObject remy = GameObject.Find("Remy");

        for (int i = 0; i < instance.RobotEnemy.Count; i++)
        {
            Robotenemies = Instantiate(instance.RobotEnemy[i], instance.RobotEnemySpawnPos[i]);
            instance.instantiatedObjects.Add(Robotenemies);
            Robotenemies.GetComponent<Enemy>().target = remy.transform;
            Robotenemies.GetComponent<Enemy>().enabled = false;
            Robotenemies.GetComponent<Animator>().enabled = false;

            Robotenemies.GetComponent<NavMeshAgent>().enabled = false;
            
        }

        yield return new WaitForSeconds(0.1f);

        for (int i = 0; i < instantiatedObjects.Count; i++)
        {
            instantiatedObjects[i].GetComponent<Animator>().enabled = true;
        }

        yield return new WaitForSeconds(3.0f);

        for (int i = 0; i < instantiatedObjects.Count; i++)
        {
            instantiatedObjects[i].GetComponent<Enemy>().enabled = true;
            instantiatedObjects[i].GetComponent<NavMeshAgent>().enabled = true;
        }
    }
    public IEnumerator LaserActive()
    {
        yield return new WaitForSeconds(17f); //TODO Time Change
        for (int i = 0; i < Laser.Count; i++)
        {
            Laser[i].SetActive(true);
        }
    }
    public IEnumerator CameraMove()
    {
        TMPObj.GetComponent<TextMeshProUGUI>().text = "";
        player.GetComponent<NewPlayerController>().enabled = false;
        Animator playerAnim = player.GetComponent<Animator>();
        playerAnim.SetBool("isRun", false);
        yield return new WaitForSeconds(1.0f);
        player.transform.LookAt(Robot.transform);
        mainCamera.transform.SetParent(cameraPos);

        while (Vector3.Distance(mainCamera.transform.localPosition, Vector3.zero) > 1.5f)
        {
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, cameraPos.position, moveSpeed * Time.deltaTime);
            mainCamera.transform.rotation = Quaternion.Slerp(mainCamera.transform.rotation, cameraPos.rotation, rotationSpeed * Time.deltaTime);
            yield return null;
        }
        yield return new WaitForSeconds(2.0f);
        StartCoroutine(Text1());
        yield return new WaitForSeconds(4.0f);
        TMPObj.GetComponent<TextMeshProUGUI>().text = "";
        yield return StartCoroutine(Text3());
        TMPImage.SetActive(false);
        TMPObj.GetComponent<TextMeshProUGUI>().text = "";

        yield return new WaitForSeconds(2.0f);
        SoundController.Instance.PlayObjectSoundRobot("Stand", clipRobotStand);
        StartCoroutine(SpawnEnemy());

        SoundController.Instance.PlaySoundLoopRobot("RobotEnemyDrop", clipRobotEnemyDrop, 5f);

        StartCoroutine(CameraShowRobotEnemy());

        yield return new WaitForSeconds(5.0f);
        mainCamera.transform.SetParent(socket);
        mainCamera.transform.localPosition = Vector3.zero;
        mainCamera.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);

        cameraPos.transform.rotation  = Quaternion.Euler(0f, 0f, 0f);

        player.GetComponent<NewPlayerController>().enabled = true;
    }
    IEnumerator CameraShowRobotEnemy()
    {
        Quaternion startRotation = cameraPos.transform.rotation;
        Quaternion targetRotation = Quaternion.Euler(11.5f, 0f, 0f);
        float elapsedTime = 0f;
        float rotationDuration = 2f;
        while (elapsedTime < rotationDuration)
        {
            cameraPos.transform.rotation = Quaternion.Slerp(startRotation, targetRotation, elapsedTime / rotationDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

    }

    IEnumerator CameraMove2()
    {
        yield return new WaitForSeconds(1.0f);
        player.transform.LookAt(Robot.transform);
        mainCamera.transform.SetParent(cameraPos);

        while (Vector3.Distance(mainCamera.transform.localPosition, Vector3.zero) > 1.5f)
        {
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, cameraPos.position, moveSpeed * Time.deltaTime);
            mainCamera.transform.rotation = Quaternion.Slerp(mainCamera.transform.rotation, cameraPos.rotation, rotationSpeed * Time.deltaTime);
            yield return null;
        }
        yield return new WaitForSeconds(1.0f);
        yield return StartCoroutine(Text2());
        yield return new WaitForSeconds(1.0f);
        TMPObj.GetComponent<TextMeshProUGUI>().text = "";
        yield return StartCoroutine(Text4());
        yield return new WaitForSeconds(1.0f);
        StartCoroutine(RobotPointing());
        SoundController.Instance.PlayObjectSoundRobot("Pointing", clipRobotPointing);
        TMPImage.SetActive(false);
        TMPObj.GetComponent<TextMeshProUGUI>().text = "";
        yield return new WaitForSeconds(4.2f);
        Calendar.GetComponent<Animator>().enabled = true;
        yield return new WaitForSeconds(4.0f);
        mainCamera.transform.SetParent(socket);
        mainCamera.fieldOfView = 40;
        mainCamera.transform.localPosition = Vector3.zero;
        mainCamera.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
        TMPObj.GetComponent<TextMeshProUGUI>().text = "";
        SoundController.bgmNum = 2;
        SoundController.Instance.ResumeBackgroundMusic();
    }
    public void DestroyEnemy()
    {
        for (int i = 0; i < instantiatedObjects.Count; i++)
        {
            Destroy(instantiatedObjects[i]);
        }
        Destroy(mainRobot);
        for (int i = 0; i < Laser.Count; i++)
        {
            Laser[i].SetActive(false);
        }
        instantiatedObjects.Clear();
        StartRobot();
    }

    public void ClearRobotStage()
    {
        if (RobotCount.Count == 0 && hasClearedRobot)
        {
            SoundController.Instance.MuteBackgroundMusic();
            ResetHealth();
            hasClearedRobot = false;
            for (int i = 0; i < Laser.Count; i++)
            {
                Laser[i].SetActive(false);
            }
            Animator robotAnim = mainRobot.GetComponent<Animator>();
            robotAnim.SetBool("Clear", true);
            Invoke("RobotDown", 1f);
            StartCoroutine(CameraMove2());


        }
    }
    void RobotDown()
    {
        SoundController.Instance.PlayObjectSoundRobot("Down", clipRobotDown);
    }
    IEnumerator RotateCamera()
    {
        Quaternion startRotation = transform.rotation;
        Quaternion targetRotation = Quaternion.Euler(-12f, -33f, 0);
        mainCamera.fieldOfView = 50;
        while (Quaternion.Angle(transform.rotation, targetRotation) > 0.1f)
        {
            cameraPos.transform.rotation = Quaternion.RotateTowards(cameraPos.transform.rotation, targetRotation, 30f * Time.deltaTime);
            yield return null;
        }
    }
    void ResetHealth()
    {
        player.GetComponent<NewPlayerController>().currentHp = 200;
        player.GetComponent<NewPlayerController>().hpUI.hp = player.GetComponent<NewPlayerController>().currentHp;

    }
    IEnumerator RobotPointing()
    {
        Animator robotAnim = mainRobot.GetComponent<Animator>();
        robotAnim.SetBool("Point", true);
        yield return new WaitForSeconds(2.0f);
        StartCoroutine(RotateCamera());
        yield return new WaitForSeconds(2.0f);
        laser3.SetActive(true);
    }
    IEnumerator Text1()
    {
        TMPImage.SetActive(true);
        text = TalkManager.table.datas[1].Text[language];
        int cur = 0;
        SoundController.Instance.PlaySoundLoopRobot("RobotVoice", clipRobotVoiceFull, 5.5f);
        while (cur < text.Length)
        {
            myLabel.text += text[cur++];
            yield return new WaitForSeconds(0.02f);
        }
        yield return new WaitForSeconds(1.5f);
    }
    IEnumerator Text2()
    {
        TMPImage.SetActive(true);
        text = TalkManager.table.datas[2].Text[language];
        int cur = 0;
        SoundController.Instance.PlaySound("RobotVoice", clipRobotVoiceFull);
        while (cur < text.Length)
        {
            myLabel.text += text[cur++];
            yield return new WaitForSeconds(0.02f);
        }
        yield return new WaitForSeconds(1.5f);
    }

    IEnumerator Text3()
    {
        TMPImage.SetActive(true);
        text = TalkManager.table.datas[23].Text[language];
        int cur = 0;
        SoundController.Instance.PlaySound("RobotVoice", clipRobotVoiceFull);
        while (cur < text.Length)
        {
            myLabel.text += text[cur++];
            yield return new WaitForSeconds(0.02f);
        }
        yield return new WaitForSeconds(1.5f);
    }

    IEnumerator Text4()
    {
        TMPImage.SetActive(true);
        text = TalkManager.table.datas[24].Text[language];
        int cur = 0;
        SoundController.Instance.PlaySound("RobotVoice", clipRobotVoiceFull);
        while (cur < text.Length)
        {
            myLabel.text += text[cur++];
            yield return new WaitForSeconds(0.02f);
        }
        yield return new WaitForSeconds(1.5f);
    }
}
