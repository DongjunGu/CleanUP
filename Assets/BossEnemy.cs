using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BossEnemy : MonoBehaviour
{
    public static bool isBossDead = false;
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
    public Camera mainCamera;
    public Transform socket;
    public Transform bossCameraPos;
    public GameObject[] folders;
    public GameObject chrome;
    public Transform KnockBackPosition;
    public GameObject virusCollect;
    private List<GameObject> spawnedFolders = new List<GameObject>();
    private bool isCharged = false;
    GameObject virusObj1;
    GameObject virusObj2;
    GameObject virusObj3;
    GameObject prefab;
    GameObject hpPrefab;
    HpBarUI hpUI;
    Animator anim;
    Material skinned_mat;
    Material original_mat;
    Color original_color;
    Vector3 originPos;
    int countDamage = 0;
    int basicHitCount = 0;
    public UnityEngine.Events.UnityEvent BossDead;
    void Awake()
    {
        originPos = transform.position;
        anim = GetComponent<Animator>();
        SkinnedMeshRenderer skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        if (skinnedMeshRenderer != null)
        {
            original_mat = skinnedMeshRenderer.material;
            original_color = skinnedMeshRenderer.material.color;
        }

    }
    void OnEnable()
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

    public void RestartBoss()
    {
        Destroy(hpPrefab);
        StopAllCoroutines();
        DestroyVirus();
        DestroySpawnedFolders();
        chrome.GetComponent<ChromeEnemy>().enabled = false;
        InActiveChrome();
        anim.SetTrigger("playIdle");
        anim.SetBool("isWalk", false);
        anim.SetBool("isBasicAttack", false);
        anim.SetBool("isMagicAttack", false);
        GetComponent<Animator>().enabled = false;
        countDamage = 0;
        transform.position = originPos;
        currentHp = 2000;
        hpUI.hp = currentHp;
        transform.localRotation = Quaternion.Euler(0f, 180f, 0f);

    }

    IEnumerator ActiveBoss()
    {
        int index = 18;
        yield return new WaitForSeconds(2.0f);
        yield return StartCoroutine(SpawnFolders());
        yield return StartCoroutine(ThrowFolder());

        DestroySpawnedFolders();
        yield return StartCoroutine(Damaged2times()); //두번 데미지 입히면
        player.GetComponent<NewPlayerController>().enabled = false;
        hpPrefab.SetActive(false);
        yield return new WaitForSeconds(1.0f);

        yield return StartCoroutine(CameraMove());
        anim.SetBool("isClapping", true);
        yield return StartCoroutine(PrintText(index++)); //우연
        anim.SetBool("isClapping", false);
        yield return StartCoroutine(PrintText(index++)); //다시도전
        anim.SetBool("isPunch", true); //플레이어 넉백
        yield return new WaitForSeconds(1.0f);
        TMPImage.SetActive(false);
        yield return StartCoroutine(CameraMovePlayer());
        StartCoroutine(KnockBack());
        yield return new WaitForSeconds(2.0f);
        anim.SetBool("isPunch", false);
        hpPrefab.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        StartCoroutine(ActiveChrome());
        yield return StartCoroutine(SpawnFolders());
        yield return StartCoroutine(ThrowFolder());
        yield return StartCoroutine(Damaged4times());
        InActiveChrome();
        hpPrefab.SetActive(false);
        yield return StartCoroutine(PrintText(index++)); // 20 직접처리하겠다
        TMPImage.SetActive(false);
        hpPrefab.SetActive(true);
        yield return StartCoroutine(ChasePlayer());


    }
    IEnumerator KnockBack()
    {
        while (Vector3.Distance(player.transform.position, KnockBackPosition.position) > 1.5f)
        {
            player.transform.position = Vector3.MoveTowards(player.transform.position, KnockBackPosition.transform.position, 40f * Time.deltaTime);
            yield return null;
        }
    }
    IEnumerator ChasePlayer()
    {
        float chargeTime = 3f;
        while (true)
        {
            Vector3 dir = player.transform.position - transform.position;
            dir.y = 0f;
            dir = dir.normalized;
            transform.position += dir * moveSpeed * Time.deltaTime;
            anim.SetBool("isWalk", true);
            Quaternion lookRotation = Quaternion.LookRotation(dir);
            float yRotation = lookRotation.eulerAngles.y;
            Quaternion currentRotation = transform.rotation;
            Quaternion targetRotation = Quaternion.Euler(0f, yRotation, 0f);
            transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, Time.deltaTime * 10.0f);

            chargeTime -= Time.deltaTime;
            if (chargeTime <= 0f)
            {
                isCharged = true;
                chargeTime = 3f;
            }

            if (basicHitCount % 5 == 0)
            {
                yield return StartCoroutine(MagicAttack());
                basicHitCount++;
            }


            if (isCharged)
            {
                yield return StartCoroutine(MagicAttack());
                isCharged = false;
            }
            yield return StartCoroutine(BasicAttack());

            yield return null;
        }
    }
    public void DestroyVirus()
    {
        GameObject[] VirusObjs = GameObject.FindGameObjectsWithTag("Virus");
        foreach (GameObject obj in VirusObjs)
        {
            Destroy(obj);
        }
    }
    IEnumerator basicAttackDelay()
    {
        yield return new WaitForSeconds(0.1f);
        basicAttackCollider.enabled = true;

        yield return new WaitForSeconds(0.6f);
        basicAttackCollider.enabled = false;
    }
    IEnumerator BasicAttack()
    {
        float dist = Vector3.Distance(transform.position, player.transform.position);
        //떄리기전 회전
        Vector3 dir = (player.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        float yRotation = lookRotation.eulerAngles.y;
        transform.rotation = Quaternion.Euler(0f, yRotation, 0f);
        if (dist < 5.0f) //Attack
        {
            anim.SetBool("isWalk", false);
            StartCoroutine(basicAttackDelay());
            anim.SetBool("isBasicAttack", true);
            basicHitCount++;
            yield return new WaitForSeconds(1.0f);
            anim.SetBool("isBasicAttack", false);
            yield return new WaitForSeconds(2.0f);
        }
    }

    IEnumerator MagicAttack()
    {
        float dist = Vector3.Distance(transform.position, player.transform.position);

        anim.SetBool("isWalk", false);
        anim.SetBool("isMagicAttack", true);
        yield return new WaitForSeconds(1.0f);
        anim.SetBool("isMagicAttack", false);

        virusObj1 = Instantiate(virus, transform.position + Vector3.up * 7f + Vector3.back * 2f, Quaternion.identity);
        virusObj2 = Instantiate(virus, transform.position + Vector3.up * 7f + Vector3.back * 2f + Vector3.right, Quaternion.identity);
        virusObj3 = Instantiate(virus, transform.position + Vector3.up * 7f + Vector3.back * 2f + Vector3.left, Quaternion.identity);
        virusObj1.transform.SetParent(virusCollect.transform);
        virusObj2.transform.SetParent(virusCollect.transform);
        virusObj3.transform.SetParent(virusCollect.transform);
        Vector3 dir = (player.transform.position - virusObj1.transform.position).normalized;
        virusObj1.GetComponent<Rigidbody>().velocity = (dir * 100f);
        virusObj2.GetComponent<Rigidbody>().velocity = (dir * 100f);
        virusObj3.GetComponent<Rigidbody>().velocity = (dir * 100f);
        isCharged = false;
    }
    IEnumerator RepeatCharging()
    {
        while (!isCharged)
        {
            yield return new WaitForSeconds(5f);
            isCharged = true;
            yield return null;
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
            countDamage++;
        }
        else
        {
            original_mat.color = Color.gray;
            basicAttackCollider.enabled = false;
            anim.SetBool("isDie", true);
            Destroy(hpPrefab, 2);
            yield return new WaitForSeconds(2.0f);
            StopAllCoroutines();
            BossDead?.Invoke();

        }
    }
    IEnumerator Damaged2times()
    {
        while (countDamage < 2)
        {
            yield return null;
        }
    }
    IEnumerator Damaged4times()
    {
        while (countDamage < 4)
        {
            yield return null;
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
            currentHp -= 50;
            hpUI.takeDamage(50);
        }
        StartCoroutine(ChangeColor());
        StartCoroutine(Damaged());
    }

    IEnumerator SpawnFolders()
    {
        for (int i = 0; i < folders.Length; i++)
        {
            GameObject newFolder = Instantiate(folders[i]);
            spawnedFolders.Add(newFolder);
        }
        yield return new WaitForSeconds(2.0f);

    }
    IEnumerator ThrowFolder()
    {
        foreach (GameObject obj in spawnedFolders)
        {
            Vector3 dir = (player.transform.position - obj.transform.position).normalized;
            obj.GetComponent<Rigidbody>().velocity = (dir * 100f);
            yield return new WaitForSeconds(1.0f);
            obj.SetActive(false);
        }
    }
    public void DestroySpawnedFolders()
    {
        if (spawnedFolders != null)
        {
            foreach (GameObject folder in spawnedFolders)
            {
                Destroy(folder);
            }
            spawnedFolders.Clear();
        }
    }

    IEnumerator ActiveChrome()
    {
        chrome.SetActive(true);
        yield return new WaitForSeconds(5.0f);
        chrome.GetComponent<ChromeEnemy>().enabled = true;
    }
    void InActiveChrome()
    {
        chrome.SetActive(false);
    }
    private void OnAnimatorMove()
    {
        Vector3 pos = anim.deltaPosition;
        Quaternion rot = anim.deltaRotation;
    }

    IEnumerator PrintText(int index)
    {
        TMPImage.SetActive(true);
        myLabel.alignment = TextAlignmentOptions.Center;
        text = TalkManager.table.datas[index].Text[language];
        int cur = 0;

        while (cur < text.Length)
        {
            myLabel.text += text[cur++];
            yield return new WaitForSeconds(0.02f);
        }
        yield return new WaitForSeconds(3.0f);
        TMPObj.GetComponent<TextMeshProUGUI>().text = "";
    }
    public IEnumerator CameraMove()
    {
        Animator playerAnim = player.GetComponent<Animator>();
        playerAnim.SetBool("isRun", false);
        yield return new WaitForSeconds(0.01f);
        mainCamera.transform.SetParent(bossCameraPos);

        while (Vector3.Distance(mainCamera.transform.localPosition, Vector3.zero) > 1.5f)
        {
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, bossCameraPos.position, 1.0f * Time.deltaTime);
            mainCamera.transform.rotation = Quaternion.Slerp(mainCamera.transform.rotation, bossCameraPos.rotation, 1.0f * Time.deltaTime);
            yield return null;
        }
    }
    public IEnumerator CameraMovePlayer()
    {
        mainCamera.transform.SetParent(socket);
        mainCamera.transform.localPosition = Vector3.zero;
        mainCamera.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
        player.GetComponent<NewPlayerController>().enabled = true;
        yield return null;
    }
}
