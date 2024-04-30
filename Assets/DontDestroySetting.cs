using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroySetting : MonoBehaviour
{
    public static DontDestroySetting _instance;
    public static DontDestroySetting Instance { get { return _instance; } }
    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        _instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
}
