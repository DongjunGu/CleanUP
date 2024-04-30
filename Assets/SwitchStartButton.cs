using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchStartButton : MonoBehaviour
{
   public void StartScene()
    {
        SceneManager.LoadScene("CleanUPMain");
    }
}
