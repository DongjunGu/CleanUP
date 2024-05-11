using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct TALKDATA
{
    public int Key;
    public string[] Text;
}

[System.Serializable]
public struct TALKTABLE
{
    public TALKDATA[] datas;
}

public class TalkManager : MonoBehaviour
{
    public static TALKTABLE table;
    void Awake()
    {
        string txt = File.ReadAllText(Application.dataPath + "/textTable.json");

        table = JsonUtility.FromJson<TALKTABLE>(txt);
        
    }
}
