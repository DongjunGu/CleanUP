using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public int maxHP;
    public int currentHp;
    public Transform target;
    private bool canTakeDamage = true;

    Rigidbody rigid;
    BoxCollider boxCol;
    Material mat;
    NavMeshAgent nav;
    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        boxCol = GetComponent<BoxCollider>();
        mat = GetComponentInChildren<SkinnedMeshRenderer>().material;
        nav = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        nav.SetDestination(target.position);
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "basicAttack" && canTakeDamage)
        {
            Weapons weapons = other.GetComponent<Weapons>();
            currentHp -= weapons.damage;
            Vector3 knockBack = transform.position - other.transform.position;
            Debug.Log(currentHp);
            StartCoroutine(DamageCooldown());
            StartCoroutine(Damaged(knockBack));
        }
    }
    IEnumerator DamageCooldown() //0.5�ʸ��� ������ ������
    {
        canTakeDamage = false;
        yield return new WaitForSeconds(0.5f); // 1�� �������� �����ϵ��� ���� (������ �ð����� ���� ����)
        canTakeDamage = true;
    }

    IEnumerator Damaged(Vector3 knockBack)
    {
        mat.color = Color.black;
        yield return new WaitForSeconds(0.1f);

        if (currentHp > 0)
        {
            mat.color = Color.white;

            knockBack = knockBack.normalized;
            knockBack += Vector3.up;
            rigid.AddForce(knockBack * 10, ForceMode.Impulse);
        }
        else
        {
            mat.color = Color.red;
            gameObject.layer = 8; //DeadEnemy
            //�˹�
            Destroy(gameObject, 1);
        }
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
