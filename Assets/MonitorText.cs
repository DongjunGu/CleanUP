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
    public GameObject LaserEnemy;
    public Transform Spawnpoint;
    void Start()
    {
        Animator monitorAnim = MonitorAnim.GetComponent<Animator>();
        int index = 4;
        StartCoroutine(WelcomeText(() =>
        {

            StartCoroutine(PrintText(index++, () =>
             {
                 StartCoroutine(CountNumber(() =>
                 {
                     StartCoroutine(PrintText(index++, () =>
                     {
                         ClearText(); MonitorAnim.SetActive(true); monitorAnim.SetBool("isAngry", true);
                         StartCoroutine(SpawnLaserEnemy(() =>
                         {
                             StartCoroutine(CountNumber(() =>
                             {
                                 ClearText();
                             }));

                         })); MonitorAnim.SetActive(false); ClearText(); 
                     })); Almondzone.SetActive(false); DestroySpawnedObjects();
                 }));
                 Almondzone.SetActive(true);
             }));

        }));
    }
    void ClearText()
    {
        monitorText.text = "";
    }
    IEnumerator WelcomeText(UnityAction done)
    {
        text = TalkManager.table.datas[3].Text[language];
        int cur = 0;

        while (cur < text.Length)
        {
            monitorText.text += text[cur++];
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(1.5f);
        done?.Invoke();
    }
    IEnumerator PrintText(int index, UnityAction done)
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
        done?.Invoke();
        
    }

    IEnumerator CountNumber(UnityAction done)
    {
        ClearText();
        int count = 10;
        while (count >= 0)
        {
            monitorText.text = count.ToString();
            count--;
            yield return new WaitForSeconds(1f);
        }
        yield return new WaitForSeconds(2.0f);
        done?.Invoke();
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

    void SpawnLaserEnemy()
    {
        Instantiate(LaserEnemy, Spawnpoint.transform.position, Quaternion.identity);
        
    }
    IEnumerator SpawnLaserEnemy(UnityAction done)
    {
        Instantiate(LaserEnemy, Spawnpoint.transform.position, Quaternion.identity);
        LaserEnemy.GetComponent<Animator>().enabled = false;
        yield return new WaitForSeconds(2.0f);
        LaserEnemy.GetComponent<Animator>().enabled = true;
        done?.Invoke();
    }
}
