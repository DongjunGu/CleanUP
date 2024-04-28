using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class ChessController : MonoBehaviour
{
    GameObject prefab;
    GameObject WhiteBishopEnemy;
    public GameObject Drawer;
    public Transform target;
    public GameObject player;
    public List<GameObject> chessEnemy;
    public List<Transform> spawnTargets;
    private static ChessController instance;
    private List<GameObject> instantiatedObjects = new List<GameObject>();
    public Camera mainCamera;
    public Transform cameraPos;
    public Transform socket;
    public float moveSpeed = 1.0f;
    public float rotationSpeed = 1.0f;
    bool hasClearedChess = false;
    public AudioClip clipDrawer;
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

    public static ChessController Instance
    {
        get
        {
            if (instance == null)
                Debug.Log("NO INSTANCE");
            return instance;
        }
    }
    void Start()
    {
        SpawnEnemy();


    }

    void Update()
    {
        if (ChessPawn.Count == 9 && !hasClearedChess)
        {
            ChessClear();
            StartCoroutine(CameraMove());
            hasClearedChess = true;
        }

    }

    void ChessClear()
    {

        player.GetComponent<NewPlayerController>().currentHp = 200;
        player.GetComponent<NewPlayerController>().hpUI.hp = player.GetComponent<NewPlayerController>().currentHp;

    }

    public static void SpawnEnemy()
    {
        GameObject remy = GameObject.Find("Remy");

        for (int i = 0; i < instance.chessEnemy.Count; i++)
        {
            GameObject instantiatedObject = Instantiate(instance.chessEnemy[i], instance.spawnTargets[i]);
            instantiatedObject.GetComponent<Enemy>().target = remy.transform;
            instance.instantiatedObjects.Add(instantiatedObject);
        }
    }
    public static void DestroyEnemy()
    {
        foreach (GameObject obj in instance.instantiatedObjects)
        {
            Destroy(obj);
        }
        instance.instantiatedObjects.Clear();

    }

    IEnumerator CameraMove()
    {
        CameraMode.IsGamePause = true;
        yield return new WaitForSeconds(1.0f);
        SoundController.bgmNum = 2;
        mainCamera.transform.SetParent(cameraPos);

        while (Vector3.Distance(mainCamera.transform.localPosition, Vector3.zero) > 1.5f)
        {
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, cameraPos.position, moveSpeed * Time.deltaTime);
            mainCamera.transform.rotation = Quaternion.Slerp(mainCamera.transform.rotation, cameraPos.rotation, rotationSpeed * Time.deltaTime);
            yield return null;
        }
        //yield return new WaitForSeconds(1.0f);

        Drawer.GetComponent<Animator>().enabled = true;
        SoundController.Instance.PlaySound("Drawer", clipDrawer);
        yield return new WaitForSeconds(4.0f);
        mainCamera.transform.SetParent(socket);
        mainCamera.transform.localPosition = Vector3.zero;
        mainCamera.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);

        
        CameraMode.IsGamePause = false;
    }
}
