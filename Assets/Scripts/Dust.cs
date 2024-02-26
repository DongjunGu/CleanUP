using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dust : MonoBehaviour
{
    public GameObject meshObject;
    public GameObject effectObject;
    public Rigidbody rigid;
    public Collider dustCollider;

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
                Debug.Log("즉발");
                yield break;
            }
            yield return null;
        }
        ExecuteExplosion();
        Debug.Log("딜레이");
    }

    void ExecuteExplosion()
    {
        rigid.velocity = Vector3.zero;
        rigid.angularVelocity = Vector3.zero;
        meshObject.SetActive(false);
        effectObject.SetActive(true);

        RaycastHit[] rayHits = Physics.SphereCastAll(transform.position, 15, Vector3.up, 0f, LayerMask.GetMask("Enemy"));
        foreach (RaycastHit hitObject in rayHits)
        {
            hitObject.transform.GetComponent<Enemy>().HitByDust(transform.position);
        }

        Destroy(gameObject, 3);
    }
    
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Debug.Log("충돌");
            ExecuteExplosion();
        }
    }
}
