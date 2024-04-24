using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class MouseEnemy : MonoBehaviour
{
    public TMPro.TMP_Text myLabel;
    public GameObject TMPImage;
    public string text;
    public int language;
    public GameObject TMPObj;
    public GameObject targetPosition;
    public int maxHP;
    public int currentHp;
    public int damage;
    private bool canTakeDamage = true;
    Rigidbody rigid;
    Material skinned_mat;
    Material original_mat;
    Color original_color;
    GameObject prefab;
    GameObject hpPrefab;
    HpBarUI hpUI;
    public UnityEngine.Events.UnityEvent MouseClear;
    void Awake()
    {
        MeshRenderer renderer = GetComponent<MeshRenderer>();
        rigid = GetComponent<Rigidbody>();
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
        myLabel = SceneData.Inst.mouseLabel;
        TMPImage = SceneData.Inst.mouseImage;
        TMPObj = SceneData.Inst.mouseText;
        targetPosition = SceneData.Inst.mouseTarget;
        MouseClear = SceneData.Inst.MouseClear;
    }
    void Start()
    {
        StartCoroutine(MouseControl());
        damage = 50;
    }
    void SpawnHPBar()
    {
        prefab = Resources.Load("HpbarMouse") as GameObject;
        hpPrefab = MonoBehaviour.Instantiate(prefab, HpBarCanvas.Root) as GameObject;
        hpUI = hpPrefab.GetComponent<HpBarUI>();
        hpUI.hp = currentHp;
        hpUI.maxHP = maxHP;

    }
    IEnumerator MouseControl()
    {
        yield return StartCoroutine(Text1());
        yield return StartCoroutine(Text3());
        myLabel.text = "";
        SpawnHPBar();
        yield return StartCoroutine(MouseRush1(30));
        yield return StartCoroutine(MouseRush1(35));
        yield return StartCoroutine(MouseRush1(40));
        yield return StartCoroutine(MouseRush1(45));
        yield return StartCoroutine(MouseRush1(50));
        hpPrefab.SetActive(false);
        yield return StartCoroutine(Text2()); //overLoaded
        hpPrefab.SetActive(true);
        yield return StartCoroutine(Waiting());
        hpPrefab.SetActive(false);
        yield return StartCoroutine(Text3()); //추격
        hpPrefab.SetActive(true);
        yield return StartCoroutine(MouseRush1(40));
        yield return StartCoroutine(MouseRush1(45));
        yield return StartCoroutine(MouseRush1(50));
        hpPrefab.SetActive(false);
        yield return StartCoroutine(Text2()); //overLoaded
        hpPrefab.SetActive(true);
        yield return StartCoroutine(Victory());
    }
    private void Update()
    {
    }
    public void OnDestroy()
    {
        if (hpPrefab != null)
            Destroy(hpPrefab);
        if(TMPImage != null)
        {
            TMPImage.SetActive(false);
            TMPObj.GetComponent<TextMeshProUGUI>().text = "";
        }
        MouseClear?.Invoke();

    }
    //public void NextStep()
    //{
    //    MouseClear?.Invoke();
    //}
    IEnumerator Waiting()
    {
        while (currentHp > 250)
        {
            yield return null;
        }
    }
    IEnumerator Victory()
    {
        while (currentHp > 0)
        {
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
        }

        else
        {
            original_mat.color = Color.gray;
            gameObject.layer = 8;

            Destroy(gameObject, 2);
            Destroy(hpPrefab, 2);
            //NextStep();
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
        StartCoroutine(Damaged());

    }
    IEnumerator Text1()
    {
        yield return new WaitForSeconds(2.0f);
        TMPImage.SetActive(true);
        myLabel.alignment = TextAlignmentOptions.Center;
        text = TalkManager.table.datas[12].Text[language];
        int cur = 0;

        while (cur < text.Length)
        {
            myLabel.text += text[cur++];
            yield return new WaitForSeconds(0.02f);
        }
        yield return new WaitForSeconds(5.0f);
        TMPImage.SetActive(false);
        TMPObj.GetComponent<TextMeshProUGUI>().text = "";
    }

    IEnumerator Text2()
    {
        TMPImage.SetActive(true);
        text = TalkManager.table.datas[13].Text[language];
        int cur = 0;

        while (cur < text.Length)
        {
            myLabel.text += text[cur++];
            yield return new WaitForSeconds(0.02f);
        }
        yield return new WaitForSeconds(5.0f);
        TMPImage.SetActive(false);
        TMPObj.GetComponent<TextMeshProUGUI>().text = "";
    }
    IEnumerator Text3()
    {
        yield return new WaitForSeconds(2.0f);
        TMPImage.SetActive(true);
        text = "마우스가 당신을 추격합니다";
        int cur = 0;

        while (cur < text.Length)
        {
            myLabel.text += text[cur++];
            yield return new WaitForSeconds(0.001f);
        }
        yield return new WaitForSeconds(2.0f);
        TMPImage.SetActive(false);
        TMPObj.GetComponent<TextMeshProUGUI>().text = "";
    }
    IEnumerator Text4()
    {
        yield return new WaitForSeconds(2.0f);
        TMPImage.SetActive(true);
        text = "승리!";
        int cur = 0;

        while (cur < text.Length)
        {
            myLabel.text += text[cur++];
            yield return new WaitForSeconds(0.001f);
        }
        yield return new WaitForSeconds(2.0f);
        TMPImage.SetActive(false);
        TMPObj.GetComponent<TextMeshProUGUI>().text = "";
    }
    IEnumerator MouseRush1(int speed)
    {
        StartCoroutine(RotateMouse());
        NavMeshPath path = new NavMeshPath();
        NavMesh.CalculatePath(transform.position, targetPosition.transform.position, -1, path);

        if (path.corners.Length > 0)
        {
            while (Vector3.Distance(transform.position, path.corners[1]) > 1.5f)
            {
                transform.position = Vector3.MoveTowards(transform.position, path.corners[1], speed * Time.deltaTime);
                yield return null;
            }
        }
        yield return new WaitForSeconds(3.0f);

    }

    IEnumerator RotateMouse()
    {
        while (true)
        {
            Vector3 direction = targetPosition.transform.position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(direction);

            float rotationTime = 0.5f;
            Quaternion startRotation = transform.rotation;
            float elapsedTime = 0;
            while (elapsedTime < rotationTime)
            {
                transform.rotation = Quaternion.Slerp(startRotation, rotation, (elapsedTime / rotationTime));
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            yield break;
        }
    }
}
