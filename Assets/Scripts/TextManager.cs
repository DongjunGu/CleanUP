using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextManager : MonoBehaviour
{
    public GameObject textBox;
    public Text talkText;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            textBox.SetActive(true);
            talkText.text = "Hello Hello Hello Hello Hello Hello Hello Hello Hello Hello Hello Hello Hello Hello Hello Hello Hello Hello Hello Hello ";
        }
    }
    private void OnTriggerExit(Collider other)
    {
        textBox.SetActive(false);
    }
}
