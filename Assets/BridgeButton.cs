using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeButton : MonoBehaviour
{
    public GameObject bridgeButton;
    private bool bridgeActivated = false;
    void Update()
    {
        ActiveBridge();
    }

    void ActiveBridge()
    {
        if (bridgeActivated)
        {
            return;
        }

        if (Pushable.allBlockSet)
        {
            bridgeButton.SetActive(true);
            bridgeActivated = true;
        }
    }
}
