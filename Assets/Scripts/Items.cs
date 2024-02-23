using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items : MonoBehaviour
{
    public enum Type { Broom, Hammer , Dust};
    public Type type;
    public int value;
    private void Update()
    {
        transform.Rotate(Vector3.up, 25.0f * Time.deltaTime);
    }
}
