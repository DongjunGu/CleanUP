using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotCount : MonoBehaviour
{
    public int robotCount;
    public static int Count;
    void Awake()
    {
        Count++;
    }

    private void OnDestroy()
    {
        Count--;
        if (robotCount == 1)
            RobotZone.hasClearedRobot = true;

    }

    void Update()
    {
        robotCount = Count;
    }
}
