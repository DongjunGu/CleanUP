using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FolderEnemy : MonoBehaviour
{
    bool _isDamaged = false;
    //public AudioClip clipFolder;
    //private void Start()
    //{
    //    SoundController.Instance.PlayBossSound("Folder",clipFolder,clipFolder)
    //}
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (!_isDamaged)
            {
                collision.gameObject.GetComponent<NewPlayerController>().currentHp -= 10;
                collision.gameObject.GetComponent<NewPlayerController>().hpUI.takeDamage(10);
                StartCoroutine(OnDamage());
            }
            
        }
    }

    IEnumerator OnDamage()
    {
        _isDamaged = true;
        yield return new WaitForSeconds(1.5f);
        _isDamaged = false;
    }
}
