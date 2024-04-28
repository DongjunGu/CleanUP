using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{

    [SerializeField] public float _scanRange;

    public int maxHP;
    public int currentHp;
    public int damage;
    public Transform target;
    public GameObject dustItemObject;
    public int EnemyCode;
    private bool canTakeDamage = true;
    public Vector3 enemyOrgPlace;
    bool _isDestroyed;
    bool _isDetected = false;
    Rigidbody rigid;
    BoxCollider boxCol;
    Material skinned_mat;
    Material original_mat;
    Color original_color;
    NavMeshAgent nav;
    
    GameObject prefab;
    GameObject hpPrefab;

    HpBarUI hpUI;

    void Awake()
    {

        enemyOrgPlace = transform.position;
        rigid = GetComponent<Rigidbody>();
        boxCol = GetComponent<BoxCollider>();
        nav = GetComponent<NavMeshAgent>();

        MeshRenderer renderer = GetComponent<MeshRenderer>();

        if (renderer != null)
        {
            original_mat = renderer.material;
            original_color = renderer.material.color;
        }
        else
        {
            renderer = GetComponentInChildren<MeshRenderer>();
            if (renderer != null)
            {
                original_mat = renderer.material;
                original_color = renderer.material.color;
            }
        }
    }
    void Start()
    {
        prefab = Resources.Load("HpBar") as GameObject;
        hpPrefab = MonoBehaviour.Instantiate(prefab,HpBarCanvas.Root) as GameObject;
        hpUI = hpPrefab.GetComponent<HpBarUI>();
        hpUI.target = transform.Find("BarPos");
        hpUI.hp = currentHp;
        hpUI.maxHP = maxHP;
        hpPrefab.SetActive(false);
    }

    void Update()
    {
        DetectPlayer();
        
    }
    void OnDestroy()
    {
        if (hpPrefab != null)
            Destroy(hpPrefab);  
    }
    void DetectPlayer()
    {
        float distance = (target.transform.position - transform.position).magnitude;

        if (!_isDestroyed)
        {
            if (distance <= _scanRange) //Detect
            {
                hpPrefab.SetActive(true);
                
                _isDetected = true;

                //if (!gameObject.name.Contains("Robot"))
                //{
                //    nav.SetDestination(target.position);
                //    GetComponent<AudioSource>().enabled = true;
                //}
                nav.SetDestination(target.position);
                GetComponent<AudioSource>().enabled = true;

            }

            if (_isDetected && (distance < _scanRange)) //Chase
            {
                if (!gameObject.name.Contains("Robot"))
                {
                    nav.SetDestination(target.position);
                }
                else if (gameObject.name.Contains("Robot"))
                {
                    nav.SetDestination(target.position);
                }
            }

            //if (distance >= 50.0f) //Go Back
            //{
            //    hpPrefab.SetActive(false);
            //    if (!gameObject.name.Contains("Robot"))
            //    {
            //        nav.SetDestination(enemyOrgPlace);
            //        GetComponent<AudioSource>().enabled = false;
            //    }
            //}
            if(distance > _scanRange)
            {
                if (!gameObject.name.Contains("Robot"))
                {
                    nav.SetDestination(enemyOrgPlace);
                }
                //else if (gameObject.name.Contains("Robot") && distance >= 50)
                //{
                    
                //}
                GetComponent<AudioSource>().enabled = false;
            }
        }
        
    }
    public void ResetPosition()
    {
        transform.position = enemyOrgPlace;
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "basicAttack" && canTakeDamage)
        {
            Weapons weapons = other.GetComponent<Weapons>();
            currentHp -= weapons.damage;
           
            //weapons.hitEffect.SetActive(true);
            Debug.Log(currentHp);
            StartCoroutine(DamageCooldown());
            StartCoroutine(Damaged());
            // weapons.hitEffect.SetActive(false);

            
            if(hpUI != null)
            {
                hpUI.takeDamage(weapons.damage);
            }
        }
    }
    IEnumerator DamageCooldown() //0.5초마다 데미지 입히기
    {
        canTakeDamage = false;
        yield return new WaitForSeconds(0.5f); // 1초 간격으로 감지하도록 설정
        canTakeDamage = true;
    }

    IEnumerator Damaged()
    {
        yield return new WaitForSeconds(0.1f);

        if (currentHp > 0)
        {
            StartCoroutine(ChangeColor());
            StartCoroutine(StopChasing());
        }

        else
        {
            //yield return new WaitForSeconds(0.2f);
            original_mat.color = Color.gray;
            nav.isStopped = true;
            gameObject.layer = 8;

            _isDestroyed = true;
            Destroy(gameObject, 2);
            Destroy(hpPrefab, 2);
            //아이템 드랍
            int randomRangeInt = Random.Range(0, 2);
            if (randomRangeInt < 1)
            {
                GameObject instantDust = Instantiate(dustItemObject, transform.position + Vector3.up * 2, transform.rotation);
            }
        }
    }
    IEnumerator ChangeColor()
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
    
    public void HitByDust()
    {
        hpPrefab.SetActive(true);
        currentHp -= 100;
        if (hpUI != null)
        {
            hpUI.takeDamage(100);
        }
        StartCoroutine(Damaged());

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
