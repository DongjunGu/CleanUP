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
    void Start()
    {
        StartCoroutine(MonitorTextStart());
    }

    IEnumerator MonitorTextStart()
    {
        Animator monitorAnim = MonitorAnim.GetComponent<Animator>();
        int index = 4;
        //yield return StartCoroutine(WelcomeText());
        //yield return StartCoroutine(PrintText(index++));
        //Almondzone.SetActive(true);
        //yield return StartCoroutine(CountNumber(15));
        //Almondzone.SetActive(false);
        //DestroySpawnedObjects();
        //yield return StartCoroutine(PrintText(index++));
        //ClearText();
        //MonitorAnim.SetActive(true);
        //monitorAnim.SetBool("isAngry", true);
        yield return StartCoroutine(SpawnLaserEnemy());
        MonitorAnim.SetActive(false);
        yield return StartCoroutine(CountNumber(40));
        ClearText();
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
    IEnumerator SpawnLaserEnemy()
    {
        GameObject instantiatedObject = Instantiate(LaserEnemyObj, Spawnpoint.transform.position, Quaternion.identity);
        LaserEnemy laserEnemyScript = instantiatedObject.GetComponent<LaserEnemy>();
        yield return new WaitForSeconds(3.0f);
        
        if (laserEnemyScript != null)
        {
            laserEnemyScript.enabled = true;
        }

        while(!(LaserEnemy.IsArrived))
        {
            yield return new WaitForSeconds(1f);
        }
    }
}
