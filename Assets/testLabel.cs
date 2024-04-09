using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testLabel : MonoBehaviour
{
    public TMPro.TMP_Text myLabel;
    public string text;
    private TalkManager talkManager;
    int language = 0;
    void Awake()
    {
        //text = talkManager.mainText;
        //Debug.Log(text);
        //text = talkManager.mainText;
        
        
    }

    void Update()
    {
       

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            text = TalkManager.table.datas[0].Text[language];
            StartCoroutine(Showing());
            
        }
    }
    public void PopText()
    {
        
        Debug.Log(text);
    }
    IEnumerator Showing()
    {
        int cur = 0;
        while(cur < text.Length)
        {
            myLabel.text += text[cur++];
            yield return new WaitForSeconds(0.1f);
        }
    }
}
