using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlmondZone : MonoBehaviour
{
    public Collider spawnArea;
    public GameObject SpawnPoint;
    public GameObject Almond;
    private Coroutine spawnCoroutine;
    private void OnEnable()
    {
        StartCoroutine(SpawnAndDestroyLoop());
    }
    void Start()
    {
        //StartCoroutine(SpawnAndDestroyLoop());
    }
    IEnumerator SpawnAndDestroyLoop()
    {
        while (true)
        {
            for (int i = 0; i < 15; i++)
            {
                StartCoroutine(SpawnPos());
            }
            yield return new WaitForSeconds(3f);
            DestroySpawnedObjects();

            yield return new WaitForSeconds(3f);
        }
    }
    IEnumerator SpawnPos()
    {
        Vector3 minPoint = spawnArea.bounds.min;
        Vector3 maxPoint = spawnArea.bounds.max;

        float randomX = Random.Range(minPoint.x, maxPoint.x);
        float randomZ = Random.Range(minPoint.z, maxPoint.z);

        Vector3 spawnPosition = new Vector3(randomX, minPoint.y, randomZ);

        Instantiate(SpawnPoint, spawnPosition + Vector3.up * 1.5f, Quaternion.identity);

        yield return new WaitForSeconds(1.0f);
        DropAlmond(spawnPosition);
    }
    void DropAlmond(Vector3 spawnPosition)
    {
        Quaternion randomRotation = Random.rotation;
        Instantiate(Almond, spawnPosition + Vector3.up * 50f, randomRotation).GetComponent<Rigidbody>().velocity = Vector3.down * 80f;

    }
    void DestroySpawnedObjects()
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
    }

}
