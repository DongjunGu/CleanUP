using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuizControl : MonoBehaviour
{
    public GameObject[] Paper;
    public TMPro.TMP_Text mainText;
    public TMPro.TMP_Text subText;
    public TMPro.TMP_Text thirdText;
    public GameObject subTextObj;
    public GameObject thirdTextObj;
    public string text;
    public int language;
    bool test = true;
    private void Start()
    {
        StartCoroutine(QuizStartControl());
    }
    private void Update()
    {
        //if(!test)
        //    mainText.text = $"5 + ({NewPlayerController.Papernumber}) = 8";
    }
    IEnumerator QuizStartControl()
    {
        int index = 10;
        subTextObj.SetActive(true);
        thirdTextObj.SetActive(true);
        yield return StartCoroutine(SpawnPaper());
        yield return StartCoroutine(PrintmainText(index++)); //If you fail to solve it within the time limit

        StartCoroutine(PrintQuizText1()); // 5 + ( ) = 8
        yield return StartCoroutine(CountNumber(10));
        yield return StartCoroutine(CheckAnswer(3));

        yield return StartCoroutine(PrintmainText(index++));

        StartCoroutine(PrintQuizText2()); // ( ) * 6 = 42
        yield return StartCoroutine(CountNumber(10));
        yield return StartCoroutine(CheckAnswer(7));

        
    }
    IEnumerator CheckAnswer(int answer)
    {
        if (NewPlayerController.Papernumber == answer)
        {
            thirdText.text = "CORRECT";
            yield return new WaitForSeconds(3.0f);
            subText.text = "";
            thirdText.text = "";
        }
        else
        {
            thirdText.text = "WRONG";
            yield return new WaitForSeconds(3.0f);
            subText.text = "";
            thirdText.text = "";
            //Angry Animation
            //Drop Laser
        }
    }
    IEnumerator SpawnPaper()
    {
        foreach (GameObject obj in Paper)
        {
            obj.SetActive(true); //Paper Active
        }
        yield return new WaitForSeconds(5.0f);

        foreach (GameObject obj in Paper) 
        {
            obj.GetComponent<Collider>().enabled = true; //Active Collider
        }
    }

    IEnumerator PrintmainText(int index)
    {
        mainText.text = "";
        mainText.fontSize = 24;
        text = TalkManager.table.datas[index].Text[language];
        int cur = 0;

        while (cur < text.Length)
        {
            mainText.text += text[cur++];
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(2.0f);
    }

    IEnumerator PrintQuizText1()
    {
        mainText.text = "";
        mainText.fontSize = 40;
        text = $"5 + ( ) = 8";
        int cur = 0;
        float startTime = Time.time;
        while (cur < text.Length)
        {
            mainText.text += text[cur++];
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(1f);

        mainText.text = "";
        while (Time.time - startTime <= 10f)
        {
            mainText.text = $"5 + ({NewPlayerController.Papernumber}) = 8";

            yield return null;
        }
    }
    IEnumerator PrintQuizText2()
    {
        mainText.text = "";
        mainText.fontSize = 40;
        text = $"( ) * 6 = 42";
        int cur = 0;
        float startTime = Time.time;
        while (cur < text.Length)
        {
            mainText.text += text[cur++];
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(1f);

        mainText.text = "";
        while (Time.time - startTime <= 20f)
        {
            mainText.text = $"({NewPlayerController.Papernumber}) * 6 = 42";

            yield return null;
        }
    }

    IEnumerator CountNumber(int count)
    {
        yield return new WaitForSeconds(1f);
        subText.text = "";
        while (count >= 0)
        {
            subText.text = count.ToString();
            count--;
            yield return new WaitForSeconds(1f);
        }
    }
    //void ClearText()
    //{
    //    mainText.text = "";
    //}
}
