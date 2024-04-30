using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveButton : MonoBehaviour
{
    public UnityEngine.Events.UnityEvent ButtonsAppear;

    void ButtonAppears()
    {
        ButtonsAppear?.Invoke();
    }
}
