using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LanguageToggle : MonoBehaviour
{
    public static int mainLanguage;
    public Toggle EnglishCheck;
    public Toggle KoreanCheck;

    public void SetEnglish(bool isChecked)
    {
        KoreanCheck.isOn = !isChecked;
        if (isChecked) mainLanguage = 0;
        else mainLanguage = 1;

    }

    public void SetKorean(bool isChecked)
    {
        EnglishCheck.isOn = !isChecked;
        if (isChecked) mainLanguage = 1;
        else mainLanguage = 0;
    }
}
