using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class robotSound : MonoBehaviour
{
    public AudioClip clipRobotsound;

    public void RobotSound()
    {
        SoundController.Instance.PlayLaserRobot("RobotSound", clipRobotsound);
    }
}
