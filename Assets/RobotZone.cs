using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEditor.SceneView;

public class RobotZone : MonoBehaviour
{
    public GameObject Robot;

    public List<GameObject> RobotEnemy;
    public List<Transform> RobotEnemySpawnPos;
    private static RobotZone instance;
    private List<GameObject> instantiatedObjects = new List<GameObject>();

    public List<GameObject> Laser = new List<GameObject>();
    public Camera mainCamera;
    public Transform cameraPos;
    public Transform socket;
    public float moveSpeed = 1.0f;
    public float rotationSpeed = 1.0f;
    public GameObject player;
    GameObject mainRobot;
    GameObject Robotenemies;
    bool hasClearedRobot = false;
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
            GetComponent<BoxCollider>().enabled = false;
            Debug.Log(NewPlayerController.stage);
            StartCoroutine(CameraMove());
            StartCoroutine(RobotFall());

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
            
            Debug.Log(Robotenemies.name);
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
    public IEnumerator RobotFall()
    {
        yield return new WaitForSeconds(12f);
        for (int i = 0; i < Laser.Count; i++)
        {
            Laser[i].SetActive(true);
        }
    }
    public IEnumerator CameraMove()
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
        yield return new WaitForSeconds(7.0f);
        StartCoroutine(SpawnEnemy());

        yield return new WaitForSeconds(3.0f);
        mainCamera.transform.SetParent(socket);
        mainCamera.transform.localPosition = Vector3.zero;
        mainCamera.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);


        CameraMode.IsGamePause = false;
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
        if (RobotCount.Count == 6 && !hasClearedRobot)
        {
            RobotClear();
            hasClearedRobot = true;
            for (int i = 0; i < Laser.Count; i++)
            {
                Laser[i].SetActive(false);
            }
            Animator robotAnim = mainRobot.GetComponent<Animator>();
            robotAnim.SetBool("Clear", true);
            Debug.Log("CLEAR STAGE");
        }
    }

    void RobotClear()
    {
        player.GetComponent<NewPlayerController>().currentHp = 200;
        player.GetComponent<NewPlayerController>().hpUI.hp = player.GetComponent<NewPlayerController>().currentHp;

    }

}
