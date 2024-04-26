using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CountDust : MonoBehaviour
{
    public TextMeshProUGUI textMeshProUGUI;
    private void Update()
    {
        string hasDustAsString = NewPlayerController.hasDust.ToString();
        textMeshProUGUI.text = hasDustAsString;
    }
}
