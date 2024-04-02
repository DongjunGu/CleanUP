using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnEnemy : MonoBehaviour
{
    public List<GameObject> enemyList;
    Vector3 enemyPos;
    
    void Start()
    {
        
    }
    void SaveTransform()
    {
        if (enemyList != null)
        {
            for(int i = 0; i < enemyList.Count; i++)
            {
                enemyPos = enemyList[i].transform.position;
            }
        }
    }

    public void ResetEnemy()
    {
        if (enemyList != null)
        {
            for (int i = 0; i < enemyList.Count; i++)
            {
                Enemy enemy = GetComponent<Enemy>();
                
                Debug.Log("DID IT");
            }
        }
    }
    
}
