using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotCount : MonoBehaviour
{
    public int robotCount = 7;
    public static int Count;
    void Awake()
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
        if (robotCount == 0)
            RobotZone.hasClearedRobot = true;
    }
}
