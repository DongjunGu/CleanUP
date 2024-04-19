using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class MonitorController : MonoBehaviour
{
    public GameObject target;
    public TMP_Text tmp;
    void Start()
    {
        transform.position = Camera.main.WorldToScreenPoint(target.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Camera.main.WorldToScreenPoint(target.transform.position);
    }
}
