using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessController : MonoBehaviour
{
    GameObject prefab;
    GameObject WhiteBishopEnemy;
    public Transform target;

    public List<GameObject> chessEnemy;
    public List<Transform> spawnTargets;
    private static ChessController instance;
    private List<GameObject> instantiatedObjects = new List<GameObject>();

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
    public static void SpawnEnemy()
    {
        GameObject remy = GameObject.Find("Remy");

        for (int i = 0; i < instance.chessEnemy.Count; i++)
        {
            GameObject instantiatedObject = Instantiate(instance.chessEnemy[i], instance.spawnTargets[i]);
            instantiatedObject.GetComponent<Enemy>().target = remy.transform;
            instance.instantiatedObjects.Add(instantiatedObject);
        }
        //WhiteBishopEnemy.GetComponent<Enemy>().target = remy.transform;
    }
    public static void DestroyEnemy()
    {
        foreach (GameObject obj in instance.instantiatedObjects)
        {
            Destroy(obj);
        }
        instance.instantiatedObjects.Clear();

    }
    //public void SpawnEnemy()
    //{
    //    GameObject remy = GameObject.Find("Remy");

    //    for (int i = 0; i < chessEnemy.Count; i++)
    //    {
    //        GameObject instantiatedObject = Instantiate(chessEnemy[i], spawnTargets[i]);
    //        instantiatedObjects.Add(instantiatedObject);
    //    }
    //    //WhiteBishopEnemy.GetComponent<Enemy>().target = remy.transform;
    //}
    //public void DestroyEnemy()
    //{
    //    foreach (GameObject obj in instantiatedObjects)
    //    {
    //        Destroy(obj);
    //    }
    //    instantiatedObjects.Clear();

    //}
}
