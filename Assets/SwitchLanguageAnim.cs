using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchLanguageAnim : MonoBehaviour
{
    public GameObject Cut1;

    private void Update()
    {
        if(LanguageToggle.mainLanguage == 1)
        {
            Cut1.GetComponent<Animator>().SetTrigger("isKorean");
        }
        else if(LanguageToggle.mainLanguage == 0)
        {
            Cut1.GetComponent<Animator>().SetTrigger("isEnglish");
        }
    }
}
