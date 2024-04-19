using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpBarCanvas : MonoBehaviour
{
    public static Transform Root;

    private void Awake()
    {
        Root = transform;
    }
}
