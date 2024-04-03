using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessPawn : MonoBehaviour
{
    public int curCount;
    public static int Count;
    // Start is called before the first frame update
    void Start()
    {
        Count++;
    }

    private void OnDestroy()
    {
        Count--;
    }

    // Update is called once per frame
    void Update()
    {
        curCount = Count;
    }
}
