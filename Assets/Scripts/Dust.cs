using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dust : MonoBehaviour
{
    public GameObject meshObject;
    public GameObject effectObject;
    public Rigidbody rigid;
    public int dustDamage = 100;
    public AudioClip clipExplosion;
    void Start()
    {
        StartCoroutine(Explosion());
    }
    IEnumerator Explosion()
    {
        float elapsedTime = 0f;
        while (elapsedTime < 3f)
        {
            elapsedTime += Time.deltaTime;

            RaycastHit[] rayHits = Physics.SphereCastAll(transform.position, 3, Vector3.up, 0f, LayerMask.GetMask("Enemy"));
            if (rayHits.Length > 0)
            {
                ExecuteExplosion();
                yield break;
            }
            yield return null;
        }
        ExecuteExplosion();
    }

    void ExecuteExplosion()
    {
        SoundController.Instance.PlayType("Dust", clipExplosion, 1f);
        rigid.velocity = Vector3.zero;
        rigid.angularVelocity = Vector3.zero;
        effectObject.SetActive(true);
        meshObject.SetActive(false);

        RaycastHit[] rayHits = Physics.SphereCastAll(transform.position, 15, Vector3.up, 0f, LayerMask.GetMask("Enemy"));
        foreach (RaycastHit hitObject in rayHits)
        {
            Enemy enemyComponent =  hitObject.transform.GetComponent<Enemy>();
            MouseEnemy mouseComponent = hitObject.transform.GetComponent<MouseEnemy>();
            BossEnemy bossComponent = hitObject.transform.GetComponent<BossEnemy>();
            bool hasEnemyCompoent = enemyComponent != null;
            bool hasMouseCompoent = mouseComponent != null;
            if (hasEnemyCompoent)
                hitObject.transform.GetComponent<Enemy>().HitByDust();

            if(!hasEnemyCompoent && !bossComponent)
            {
                hitObject.transform.GetComponent<MouseEnemy>().HitByDust();
            }

            if (!hasEnemyCompoent && !hasMouseCompoent)
            {
                hitObject.transform.GetComponent<BossEnemy>().HitByDust();
            }

        }

        Destroy(gameObject, 3);
    }
    
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            ExecuteExplosion();
        }
    }
}
