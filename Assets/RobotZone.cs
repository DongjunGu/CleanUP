using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RobotZone : MonoBehaviour
{
    public GameObject Robot;
    
    public List<GameObject> RobotEnemy;
    public List<GameObject> Lazer;
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            Robot.GetComponent<Animator>().enabled = true;
            
            StartCoroutine(RobotFall());
        }
    }

    IEnumerator RobotFall()
    {
        yield return new WaitForSeconds(8f);
        for(int i = 0; i < RobotEnemy.Count; i++)
        {
            RobotEnemy[i].SetActive(true);
            
            RobotEnemy[i].GetComponent<Animator>().enabled = true;
        }
        

        yield return new WaitForSeconds(4f);
        for (int i = 0; i < RobotEnemy.Count; i++)
        {
            RobotEnemy[i].GetComponent<NavMeshAgent>().enabled = true;
            RobotEnemy[i].GetComponent<Enemy>().enabled = true;
        }
        for (int i = 0; i < Lazer.Count; i++)
        {
            Lazer[i].SetActive(true);
        }

        yield return new WaitForSeconds(0.1f);

        GetComponent<BoxCollider>().enabled = false;
    }
}
