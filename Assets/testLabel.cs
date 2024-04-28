using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class testLabel : MonoBehaviour
{
    public GameObject TMPImage;
    public GameObject TMPObj;
    public TMPro.TMP_Text myLabel;
    public string text;
    private TalkManager talkManager;
    public int language;
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            StartCoroutine(Showing());
            SoundController.bgmNum = 1;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        TMPObj.GetComponent<TextMeshProUGUI>().text = "";
        TMPImage.SetActive(false);
        GetComponent<Collider>().enabled = false;
        
    }
    IEnumerator Showing()
    {
        CameraMode.IsGamePause = true;
        TMPImage.SetActive(true);
        text = TalkManager.table.datas[0].Text[language];
        int cur = 0;
        while(cur < text.Length)
        {
            myLabel.text += text[cur++];
            yield return new WaitForSeconds(0.05f);
        }
        yield return new WaitForSeconds(1.5f);
        CameraMode.IsGamePause = false;
    }
}
