using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{

    [SerializeField] public float _scanRange;

    public int maxHP;
    public int currentHp;
    public Transform target;
    public GameObject dustItemObject;
    private bool canTakeDamage = true;

    bool _isDestroyed;
    bool _isDetected;
    Rigidbody rigid;
    BoxCollider boxCol;
    Material skinned_mat;
    Material original_mat;
    Color original_color;
    NavMeshAgent nav;
    Vector3 enemyOrgPlace;

    void Awake()
    {
        enemyOrgPlace = transform.position;
        rigid = GetComponent<Rigidbody>();
        boxCol = GetComponent<BoxCollider>();
        original_mat = GetComponent<MeshRenderer>().material;
        original_color = GetComponent<MeshRenderer>().material.color;
        //skinned_mat = GetComponentInChildren<SkinnedMeshRenderer>().material;
        
        //normal_mat = GetComponent<MeshRenderer>().material;
        nav = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        DetectPlayer();
    }

    void DetectPlayer()
    {
        float distance = (target.transform.position - transform.position).magnitude;

        if (!_isDestroyed)
        {
            if (distance <= _scanRange) //Detect
            {
                _isDetected = true;
                nav.SetDestination(target.position);
            }

            if (_isDetected && (distance < 50.0f)) //Chase
            {
                nav.SetDestination(target.position);
            }

            if (distance >= 50.0f) //Go Back
            {
                
                nav.SetDestination(enemyOrgPlace);
            }
        }
        
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "basicAttack" && canTakeDamage)
        {
            Weapons weapons = other.GetComponent<Weapons>();
            currentHp -= weapons.damage;
            StartCoroutine(ChnageColor());
            StartCoroutine(StopChasing());
            //weapons.hitEffect.SetActive(true);
            Vector3 knockBack = transform.position - other.transform.position;
            Debug.Log(currentHp);
            StartCoroutine(DamageCooldown());
            StartCoroutine(Damaged(knockBack, false));
            // weapons.hitEffect.SetActive(false);
            HealthBar healthBar = GetComponentInChildren<HealthBar>();
            if(healthBar != null)
            {
                healthBar.takeDamage(weapons.damage);
            }
        }
    }
    IEnumerator DamageCooldown() //0.5초마다 데미지 입히기
    {
        canTakeDamage = false;
        yield return new WaitForSeconds(0.5f); // 1초 간격으로 감지하도록 설정 (적절한 시간으로 조절 가능)
        canTakeDamage = true;
    }

    IEnumerator Damaged(Vector3 knockBack, bool isDusted)
    {
        yield return new WaitForSeconds(0.1f);

        if (currentHp > 0)
        {
            
        }

        else
        {
            gameObject.layer = 8; //DeadEnemy

            //normal_mat.color = Color.black;
            _isDestroyed = true;
            Destroy(gameObject, 2);
            //아이템 드랍

            int randomRangeInt = Random.Range(0, 2);
            if (randomRangeInt < 1)
            {
                GameObject instantDust = Instantiate(dustItemObject, transform.position + Vector3.up * 2, transform.rotation);
            }


        }
    }
    IEnumerator ChnageColor()
    {
        original_mat.color = Color.red;
        yield return new WaitForSeconds(0.5f);
        original_mat.color = original_color;

    }
    IEnumerator StopChasing()
    {
        nav.isStopped = true;
        yield return new WaitForSeconds(1.0f);
        nav.isStopped = false;
    }
    IEnumerator Damaged_Skinned(Vector3 knockBack, bool isDusted)
    {
        skinned_mat.color = Color.red;
        yield return new WaitForSeconds(0.1f);

        if (currentHp > 0)
        {
            skinned_mat.color = Color.white;

            //knockBack = knockBack.normalized;
            //knockBack += Vector3.up;
            //rigid.AddForce(knockBack * 10, ForceMode.Impulse);
        }

        else
        {
            gameObject.layer = 8; //DeadEnemy

            if (isDusted)
            {
                skinned_mat.color = Color.red;
                //넉백
                knockBack = knockBack.normalized;
                knockBack += Vector3.up * 50;

                rigid.freezeRotation = false;
                rigid.AddForce(knockBack * 20, ForceMode.Impulse);
                rigid.AddTorque(knockBack * 15, ForceMode.Impulse);
            }
            else
            {
                knockBack = knockBack.normalized;
                knockBack += Vector3.up;
                rigid.AddForce(knockBack * 10, ForceMode.Impulse);
            }
            skinned_mat.color = Color.black;
            _isDestroyed = true;
            Destroy(gameObject, 2);
            //아이템 드랍

            int randomRangeInt = Random.Range(0, 2);
            if (randomRangeInt < 1)
            {
                GameObject instantDust = Instantiate(dustItemObject, transform.position + Vector3.up * 2, transform.rotation);
            }


        }
    }
    public void HitByDust(Vector3 pos)
    {
        currentHp -= 100;
        Vector3 knockBack = transform.position - pos;
        Debug.Log(currentHp);
        StartCoroutine(ChnageColor());
        StartCoroutine(StopChasing());
        StartCoroutine(Damaged(knockBack, true));

    }
    private void FixedUpdate()
    {
        RotationFreeze();
    }
    void RotationFreeze()
    {
        rigid.velocity = Vector3.zero;
        rigid.angularVelocity = Vector3.zero;
    }
}
