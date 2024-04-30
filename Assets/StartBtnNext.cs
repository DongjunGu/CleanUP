using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class StartBtnNext : MonoBehaviour
{
    public GameObject cut4Image;
    public TMP_Text text;
    public void WaitandStart()
    {
        StartCoroutine(WaitTime());
    }

    IEnumerator WaitTime()
    {
        yield return new WaitForSeconds(1.0f);
        cut4Image.SetActive(true);
    }

    public void ChangeColor()
    {
        StartCoroutine(changeColor());
    }
    IEnumerator changeColor()
    {
        text.color = new Color32(53,53,53,255);
        yield return new WaitForSeconds(0.5f);
        text.color = new Color32(255,255, 255, 255);
    }
}
