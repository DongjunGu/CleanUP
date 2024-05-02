using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingPage : MonoBehaviour
{
    public static SettingPage Instance;
    public GameObject settingPage;
    public bool isOpened = false;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Update()
    {
        //if (Input.GetKey(KeyCode.Escape))
        //{
        //    if (settingPage.activeSelf && isOpened)
        //    {
        //        settingPage.SetActive(false);
        //        Invoke("Delay", 0.5f);
        //    }
        //    else if(!settingPage.activeSelf && !isOpened)
        //    {
        //        settingPage.SetActive(true);
        //        Invoke("Delay", 0.5f);
        //    }
        //}
        if (Input.GetKey(KeyCode.Escape))
        {
            if (settingPage.activeSelf)
            {
                settingPage.SetActive(false);
                isOpened = !isOpened;
            }
            else if (!settingPage.activeSelf)
            {
                settingPage.SetActive(true);
                isOpened = !isOpened;
            }
        }
    }

    public void Delay()
    {
        isOpened = !isOpened;
    }
}
