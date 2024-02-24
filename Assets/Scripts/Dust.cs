using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dust : MonoBehaviour
{
    public GameObject meshObject;
    public GameObject effectObject;
    public Rigidbody rigid;
    public Collider dustCollider;
    bool _isHit = false;

    void Start()
    {
        
          StartCoroutine(Explosion());

    }

    IEnumerator Explosion()
    {
        yield return new WaitForSeconds(3f);
        rigid.velocity = Vector3.zero;
        rigid.angularVelocity = Vector3.zero;
        meshObject.SetActive(false);
        effectObject.SetActive(true);
        Debug.Log("µÙ∑π¿Ã");
        //RaycastHit[] rayHits = Physics.SphereCastAll(transform.position, 15, Vector3.up, 0f, LayerMask.GetMask("Enemy"));
        //foreach (RaycastHit hitObject in rayHits)
        //{
        //    hitObject.transform.GetComponent<Enemy>().HitByDust(transform.position);
        //}
        //Destroy(gameObject, 3);

    }
    void ExplosionImed()
    {
        rigid.velocity = Vector3.zero;
        rigid.angularVelocity = Vector3.zero;
        meshObject.SetActive(false);
        effectObject.SetActive(true);
        Debug.Log("¡Ôπﬂ");
        RaycastHit[] rayHits = Physics.SphereCastAll(transform.position, 15, Vector3.up, 0f, LayerMask.GetMask("Enemy"));
        foreach (RaycastHit hitObject in rayHits)
        {
            hitObject.transform.GetComponent<Enemy>().HitByDust(transform.position);
        }
        Destroy(gameObject, 3);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            _isHit = true;
            ExplosionImed();
        }
    }
}
