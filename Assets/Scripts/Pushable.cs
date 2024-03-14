using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Pushable : MonoBehaviour
{
   public TextMeshPro textMeshPro;

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player")
        {
            if (textMeshPro != null)
            {
                textMeshPro.gameObject.SetActive(true);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (textMeshPro != null)
        {
            textMeshPro.gameObject.SetActive(false);
        }
    }
}
