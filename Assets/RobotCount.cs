using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotCount : MonoBehaviour
{
    public int robotCount;
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
        robotCount = Count;
    }
}
