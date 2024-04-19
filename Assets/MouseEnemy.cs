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
    void Start()
    {
        
        StartCoroutine(MouseControl());
    }
    IEnumerator MouseControl()
    {
        int index;
        StartCoroutine(Text1(index++));
        yield return new WaitForSeconds(5.0f);
        TMPImage.SetActive(false);
        TMPObj.GetComponent<TextMeshProUGUI>().text = "";
        yield return new WaitForSeconds(5.0f);

        yield return StartCoroutine(MouseRush1());
        yield return StartCoroutine(MouseRush1());
        yield return StartCoroutine(MouseRush1());
        yield return StartCoroutine(MouseRush1());
        yield return StartCoroutine(MouseRush1());
    }
    IEnumerator Text1(int index)
    {
        yield return new WaitForSeconds(2.0f);
        TMPImage.SetActive(true);
        text = TalkManager.table.datas[index].Text[language];
        int cur = 0;

        while (cur < text.Length)
        {
            myLabel.text += text[cur++];
            yield return new WaitForSeconds(0.02f);
        }
        yield return new WaitForSeconds(1.5f);
    }

    IEnumerator MouseRush1()
    {
        StartCoroutine(RotateMouse());
        NavMeshPath path = new NavMeshPath();
        NavMesh.CalculatePath(transform.position, targetPosition.transform.position, -1, path);

        if (path.corners.Length > 0)
        {
            while (Vector3.Distance(transform.position, path.corners[1]) > 1.5f)
            {
                transform.position = Vector3.MoveTowards(transform.position, path.corners[1], 50 * Time.deltaTime);
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
