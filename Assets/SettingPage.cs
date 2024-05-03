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
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (settingPage.activeSelf)
            {
                settingPage.SetActive(false);
                Time.timeScale = 1;
            }
            else if (!settingPage.activeSelf)
            {
                settingPage.SetActive(true);
                Time.timeScale = 0;
            }
        }
    }

    public void ClickSound()
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.Play();
    }
    public void XButton()
    {
        Time.timeScale = 1;
    }
}
