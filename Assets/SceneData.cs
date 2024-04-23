using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneData : MonoBehaviour
{
    public static SceneData Inst;
    public TMPro.TMP_Text mouseLabel;
    public GameObject mouseTarget;
    public GameObject mouseText;
    public GameObject mouseImage;
    public UnityEngine.Events.UnityEvent MouseClear;
    // Start is called before the first frame update
    void Start()
    {
        Inst = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
