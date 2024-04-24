using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy : MonoBehaviour
{
    public TMPro.TMP_Text myLabel;
    public GameObject TMPImage;
    public string text;
    public int language;
    public GameObject TMPObj;
    public int maxHP;
    public int currentHp;
    public int damage;
    public GameObject player;
    private bool canTakeDamage = true;
    public float moveSpeed = 5.0f;
    public BoxCollider basicAttackCollider;
    public GameObject virus;
    GameObject virusObj;
    GameObject prefab;
    GameObject hpPrefab;
    HpBarUI hpUI;
    Animator anim;
    Material skinned_mat;
    Material original_mat;
    Color original_color;
    void Awake()
    {
        anim = GetComponent<Animator>();
        SkinnedMeshRenderer skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        if (skinnedMeshRenderer != null)
        {
            original_mat = skinnedMeshRenderer.material;
            original_color = skinnedMeshRenderer.material.color;
        }

    }
    void Start()
    {
        SpawnHPBar();
        StartCoroutine(ActiveBoss());
    }
    private void Update()
    {

    }
    void SpawnHPBar()
    {
        prefab = Resources.Load("HpbarBoss") as GameObject;
        hpPrefab = MonoBehaviour.Instantiate(prefab, HpBarCanvas.Root) as GameObject;
        hpUI = hpPrefab.GetComponent<HpBarUI>();
        hpUI.hp = currentHp;
        hpUI.maxHP = maxHP;
    }

    IEnumerator ActiveBoss()
    {
        yield return StartCoroutine(ChasePlayer());
    }
    IEnumerator ChasePlayer()
    {
        while (true)
        {
            Vector3 dir = (player.transform.position - transform.position).normalized;
            transform.position += dir * moveSpeed * Time.deltaTime;

            Quaternion lookRotation = Quaternion.LookRotation(dir);
            float yRotation = lookRotation.eulerAngles.y;
            Quaternion currentRotation = transform.rotation;
            Quaternion targetRotation = Quaternion.Euler(0f, yRotation, 0f);
            transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, Time.deltaTime * 10.0f);

            yield return StartCoroutine(BasicAttack());
            yield return StartCoroutine(MagicAttack());
            Destroy(virusObj);

            yield return null;
        }
    }
    IEnumerator basicAttack()
    {
        yield return new WaitForSeconds(0.1f);
        basicAttackCollider.enabled = true;

        yield return new WaitForSeconds(0.5f);
        basicAttackCollider.enabled = false;
    }
    IEnumerator BasicAttack()
    {
        float dist = Vector3.Distance(transform.position, player.transform.position);
        //떄리기전 회전
        Vector3 dir = (player.transform.position- transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        float yRotation = lookRotation.eulerAngles.y;
        transform.rotation = Quaternion.Euler(0f, yRotation, 0f);
        if (dist < 5.0f) //Attack
        {
            anim.SetBool("isBasicAttack", true);
            StartCoroutine(basicAttack());
            yield return new WaitForSeconds(1.0f);
            anim.SetBool("isBasicAttack", false);
            yield return new WaitForSeconds(2.0f);
        }
    }

    IEnumerator MagicAttack()
    {
        float dist = Vector3.Distance(transform.position, player.transform.position);

        if (dist > 50.0f) //Attack
        {
            anim.SetBool("isMagicAttack", true);
            yield return new WaitForSeconds(1.0f);
            anim.SetBool("isMagicAttack", false);

            virusObj = Instantiate(virus, transform.position + Vector3.up * 7f + Vector3.back * 2f, Quaternion.identity);
            Vector3 dir = (player.transform.position - virusObj.transform.position).normalized;
            virusObj.GetComponent<Rigidbody>().velocity = (dir * 100f);
            yield return new WaitForSeconds(1.0f);
        }
        

    }
    void OnTriggerEnter(Collider other)
    {
        if (this.enabled)
        {
            if (other.tag == "basicAttack" && canTakeDamage)
            {
                Weapons weapons = other.GetComponent<Weapons>();
                currentHp -= weapons.damage;

                StartCoroutine(DamageCooldown());
                StartCoroutine(Damaged());


                if (hpUI != null)
                {
                    hpUI.takeDamage(weapons.damage);
                }
            }
        }
    }
    IEnumerator DamageCooldown()
    {
        canTakeDamage = false;
        yield return new WaitForSeconds(0.5f);
        canTakeDamage = true;
    }
    IEnumerator Damaged()
    {
        yield return new WaitForSeconds(0.1f);

        if (currentHp > 0)
        {
            StartCoroutine(ChangeColor());
        }
        else
        {
            original_mat.color = Color.gray;
            Destroy(gameObject, 2);
            Destroy(hpPrefab, 2);
        }
    }
    IEnumerator ChangeColor()
    {
        original_mat.color = Color.red;
        yield return new WaitForSeconds(0.5f);
        original_mat.color = original_color;
    }
    public void HitByDust()
    {
        if (hpUI != null)
        {
            currentHp -= 100;
            hpUI.takeDamage(100);
        }
        StartCoroutine(ChangeColor());
        StartCoroutine(Damaged());
    }
}
