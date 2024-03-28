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
            //StartCoroutine(BatMovement());

        if (type == Type.Hammer)
            RotateHammer();
    }
    
    public void RotateBroom()
    {
        transform.Rotate(Vector3.up, 25.0f * Time.deltaTime);
    }
    IEnumerator BatMovement()
    {
        transform.position += Vector3.up * 1.0f * Time.deltaTime;
        Debug.Log("UP");
        yield return new WaitForSeconds(2.0f);

        transform.position += Vector3.down * 1.0f * Time.deltaTime;
        Debug.Log("Down");
        yield return new WaitForSeconds(2.0f);
    }
    public void RotateHammer()
    {
        transform.Rotate(Vector3.right, 25.0f * Time.deltaTime);

    }
}
