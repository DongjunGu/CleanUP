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
    //Dictionary<int, string[]> talkdata;
    public static TALKTABLE table;
    void Awake()
    {
        //talkdata = new Dictionary<int, string[]>();
        //GenerateData();

        //Debug.Log(talkdata[100][language]);
        /*
        TALKDATA data = new TALKDATA(); ;
        data.Key = 0;
        data.Text = new string[2];
        data.Text[0] = "Hello";
        data.Text[1] = "¾È³ç";

        TALKTABLE datas = new TALKTABLE();
        datas.datas = new TALKDATA[1];
        datas.datas[0] = data;

        string txt = JsonUtility.ToJson(datas, true);

        File.WriteAllText(Application.dataPath + "/textTable.json", txt);
        */

        string txt = File.ReadAllText(Application.dataPath + "/textTable.json");

        table = JsonUtility.FromJson<TALKTABLE>(txt);
        
    }
    private void Update()
    {
       
    }
    void GenerateData()
    {
        //talkdata.Add(100, new string[] { "HELLO" , "¾È³ç" });

        
    }

    //public string GetTalk(int id, int talkIndex)
    //{
    //    return talkdata[id][talkIndex];
    //}
}
