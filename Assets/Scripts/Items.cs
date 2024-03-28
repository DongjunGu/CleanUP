using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Items : MonoBehaviour
{
    [SerializeField] GameObject weaponImage;
    public enum Type { Broom, Bat, Hammer, Dust};
    public Type type;
    public int value;
    private void Update()
    {
        if (type == Type.Broom)
            RotateBroom();
        if (type == Type.Bat)
            RotateBat();
        if (type == Type.Hammer)
            RotateHammer();
    }
    
    public void RotateBroom()
    {
        transform.Rotate(Vector3.up, 25.0f * Time.deltaTime);
    }
    public void RotateBat()
    {
        transform.Rotate(Vector3.up, 25.0f * Time.deltaTime);
    }

    public void RotateHammer()
    {
        transform.Rotate(Vector3.right, 25.0f * Time.deltaTime);

    }
}
