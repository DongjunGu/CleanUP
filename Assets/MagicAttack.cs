using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicAttack : MonoBehaviour
{
    bool _isDamaged = false;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<NewPlayerController>().currentHp -= 30;
            collision.gameObject.GetComponent<NewPlayerController>().hpUI.takeDamage(30);
            StartCoroutine(OnDamage());
        }
    }

    IEnumerator OnDamage()
    {
        _isDamaged = true;
        yield return new WaitForSeconds(1.5f);
        _isDamaged = false;
    }
}
