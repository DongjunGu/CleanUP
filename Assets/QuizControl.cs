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
    public GameObject foodDrop;
    public GameObject player;
    public string text;
    public int language;
    public AudioClip clipTyping;
    public AudioClip clipPaperSpawn;
    public AudioClip clipCorrect;
    public AudioClip clipWrong;
    public AudioClip clipCountDown;
    public UnityEngine.Events.UnityEvent MouseEnemyStart;
    void OnEnable()
    {
        StartCoroutine(QuizStartControl());
    }
    public void RestartQuiz()
    {
        foreach (GameObject obj in Paper)
        {
            obj.GetComponent<Animator>().SetBool("isShuffle", false);
            obj.SetActive(false);
        }

        foodDrop.SetActive(false);
        mainText.fontSize = 24;
        ClearMainText();
        ClearSubText();
        ClearThirdText();
        StopAllCoroutines();
    }
    private void Update()
    {

    }
    IEnumerator QuizStartControl()
    {
        int index = 7;
        //subTextObj.SetActive(true);
        //thirdTextObj.SetActive(true);
        //SoundController.Instance.PlaySoundDesk("PaperSpawn", clipPaperSpawn);
        //yield return StartCoroutine(SpawnPaper());
        
        //yield return StartCoroutine(PrintmainText(index++)); //If you fail to solve it within the time limit
        
        //StartCoroutine(PrintQuizText1()); // 5 + ( ) = 8
        //yield return StartCoroutine(CountNumber(15));
        //yield return StartCoroutine(CheckAnswer(3));

        //yield return StartCoroutine(PrintmainText(index++)); //Next Quiz

        //StartCoroutine(PrintQuizText2()); // ( ) * 9 = 81
        //yield return StartCoroutine(CountNumber(15));
        //yield return StartCoroutine(CheckAnswer(9));

        //yield return StartCoroutine(PrintmainText(index++)); //Well.. You're Good. How about this?
        //yield return StartCoroutine(ShufflePaper());

        //StartCoroutine(PrintQuizText3()); // 12 - 10 / 2 = ( ) = 7
        //yield return StartCoroutine(CountNumber(15));
        //yield return StartCoroutine(CheckAnswer(7));

        yield return StartCoroutine(PrintmainText(index++)); // 10 You're clever!
        //yield return StartCoroutine(PrintmainText(index++)); // 11 Okay..\n Go get him Mouse!

        //foreach (GameObject obj in Paper)
        //{
        //    foreach (Transform child in obj.transform)
        //    {
        //        BoxCollider childCollider = child.GetComponent<BoxCollider>();

        //        if (childCollider != null)
        //        {
        //            childCollider.enabled = false;
        //        }
        //    }
        //}
        MouseEnemyStart?.Invoke();
    }
    IEnumerator ShufflePaper()
    {
        SoundController.Instance.PlaySoundDesk("PaperSpawn", clipPaperSpawn);
        foreach (GameObject obj in Paper)
        {
            obj.GetComponent<Animator>().SetBool("isShuffle", true);
        }

        yield return new WaitForSeconds(3.0f);
    }
    IEnumerator CheckAnswer(int answer)
    {
        if (NewPlayerController.Papernumber == answer)
        {
            SoundController.Instance.PlaySoundDesk("Correct", clipCorrect);
            thirdText.text = "CORRECT";
            yield return new WaitForSeconds(3.0f);
            ClearSubText();
            ClearThirdText();
        }
        else
        {
            SoundController.Instance.PlaySoundDesk("Wrong", clipWrong);
            thirdText.text = "WRONG";
            yield return new WaitForSeconds(3.0f);
            ClearSubText();
            ClearThirdText();
            thirdText.text = $"Answer = {answer}";
            yield return new WaitForSeconds(2.0f);
            ClearSubText();
            ClearThirdText();
            //Angry Animation
            foodDrop.SetActive(true);
            yield return new WaitForSeconds(2.5f);
            NewPlayerController playerhp = player.GetComponent<NewPlayerController>();
            playerhp.currentHp -= 200;//Test 200
            playerhp.hpUI.takeDamage(200);
            yield return new WaitForSeconds(2.0f);
            foodDrop.SetActive(false);
        }
    }
    IEnumerator SpawnPaper()
    {
        foreach (GameObject obj in Paper)
        {
            obj.SetActive(true);
            obj.GetComponent<Collider>().enabled = false;
        }

        yield return new WaitForSeconds(5.0f);

        foreach (GameObject obj in Paper) 
        {
            obj.GetComponent<Collider>().enabled = true; //Active Collider
            foreach (Transform child in obj.transform)
            {
                BoxCollider childCollider = child.GetComponent<BoxCollider>();

                if (childCollider != null)
                {
                    childCollider.enabled = true;
                }
            }
        }
    }

    IEnumerator PrintmainText(int index)
    {
        ClearMainText();
        mainText.fontSize = 24;
        text = TalkManager.table.datas[index].Text[language];
        int cur = 0;

        while (cur < text.Length)
        {
            mainText.text += text[cur++];
            SoundController.Instance.PlayType("Typing", clipTyping, 0.1f);
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(2.0f);
    }

    IEnumerator PrintQuizText1()
    {
        ClearMainText();
        mainText.fontSize = 40;
        text = $"5 + ( ) = 8";
        int cur = 0;
        float startTime = Time.time;
        while (cur < text.Length)
        {
            mainText.text += text[cur++];
            SoundController.Instance.PlayType("Typing", clipTyping, 0.1f);
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(1f);

        ClearMainText();
        while (Time.time - startTime <= 16f)
        {
            string temp = NewPlayerController.Papernumber == 0? " " : NewPlayerController.Papernumber.ToString();
            mainText.text = $"5 + ({temp}) = 8";

            yield return null;
        }
    }
    IEnumerator PrintQuizText2()
    {
        ClearMainText();
        mainText.fontSize = 40;
        text = $"( ) * 9 = 81";
        int cur = 0;
        float startTime = Time.time;
        while (cur < text.Length)
        {
            mainText.text += text[cur++];
            SoundController.Instance.PlayType("Typing", clipTyping, 0.1f);
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(1f);

        ClearMainText();
        while (Time.time - startTime <= 16f)
        {
            string temp = NewPlayerController.Papernumber == 0? " " : NewPlayerController.Papernumber.ToString();
            mainText.text = $"({temp}) * 9 = 81";

            yield return null;
        }
    }

    IEnumerator PrintQuizText3()
    {
        ClearMainText();
        mainText.fontSize = 40;
        text = $"12 - 10 / 2 = ( )"; //7
        int cur = 0;
        float startTime = Time.time;
        while (cur < text.Length)
        {
            mainText.text += text[cur++];
            SoundController.Instance.PlayType("Typing", clipTyping, 0.1f);
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(1f);

        ClearMainText();

        while (Time.time - startTime <= 16f)
        {
            string temp = NewPlayerController.Papernumber == 0 ? " " : NewPlayerController.Papernumber.ToString();
            mainText.text = $"12 - 10 / 2 = ({temp})";

            yield return null;
        }
    }
    IEnumerator CountNumber(int count)
    {
        yield return new WaitForSeconds(1f);
        ClearSubText();
        while (count >= 0)
        {
            subText.text = count.ToString();
            SoundController.Instance.PlaySoundDesk("CountDown", clipCountDown);
            count--;
            yield return new WaitForSeconds(1f);
        }
    }
    void ClearMainText()
    {
        mainText.text = "";
    }
    void ClearSubText()
    {
        subText.text = "";
    }
    void ClearThirdText()
    {
        thirdText.text = "";
    }
}
