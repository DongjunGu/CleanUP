using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessPawn : MonoBehaviour
{
    public int curCount;
    public static int Count;
    
    void Start()
    {
        Count++;
    }

    private void OnDestroy()
    {
        Count--;
    }

    
    void Update()
    {
        curCount = Count;
    }
}
