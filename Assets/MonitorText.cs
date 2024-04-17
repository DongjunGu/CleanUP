using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.PlayerSettings.Switch;
using UnityEngine.Events;

public class MonitorText : MonoBehaviour
{
    public TMPro.TMP_Text monitorText;
    public string text;
    public int language;
    void Start()
    {
        StartCoroutine(WelcomeText(() => StartCoroutine(CountNumber())));
        
        //ClearText();
    }
    void ClearText()
    {
        monitorText.text = "";
    }
    IEnumerator WelcomeText(UnityAction done)
    {
        text = TalkManager.table.datas[3].Text[language];
        int cur = 0;

        while (cur < text.Length)
        {
            monitorText.text += text[cur++];
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(1.5f);
        //ClearText();
        done?.Invoke();
    }

    IEnumerator CountNumber()
    {
        ClearText();
        int count = 30;
        while (count >= 0)
        {
            monitorText.text = count.ToString();
            count--;
            yield return new WaitForSeconds(1f);
        }
    }
}
