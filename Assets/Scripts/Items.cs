using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Items : MonoBehaviour
{
    [SerializeField] GameObject weaponImage;
    public enum Type { Broom, Bat, Hammer, Dust};
    public Type type;
    public int value;
    public static bool isObtain = false;
    private void Start()
    {
        if (type == Type.Bat)
            StartCoroutine(BatMovement());
    }
    private void Update()
    {
        if (type == Type.Broom)
            RotateBroom();
        

        if (type == Type.Hammer)
            RotateHammer();
    }
    public void OnDestroy()
    {
        if(type == Type.Broom)
        {
            isObtain = true;
            //GameObject Greendoor = GameObject.Find("green_door");
            //NewPlayerController.stage++;
            //Destroy(Greendoor);
        }
    }

    public void RotateBroom()
    {
        transform.Rotate(Vector3.up, 25.0f * Time.deltaTime);
    }
    IEnumerator BatMovement()
    {
        Vector3 dir = Vector3.up;
        float playTime = 0.0f;
        while (true) 
        {
            playTime += Time.deltaTime;
            if(playTime >= 2.0f)
            {
                playTime = 0.0f;
                dir = -dir;
            }
            transform.position += dir * 1.0f * Time.deltaTime;
            yield return null;
        }
    }
    public void RotateHammer()
    {
        transform.Rotate(Vector3.right, 25.0f * Time.deltaTime);
    }
}
