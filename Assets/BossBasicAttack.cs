using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBasicAttack : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            Debug.Log("PlayerHit");
            other.gameObject.GetComponent<NewPlayerController>().currentHp -= 10;
            other.gameObject.GetComponent<NewPlayerController>().hpUI.takeDamage(10);
        }
    }
    
}
