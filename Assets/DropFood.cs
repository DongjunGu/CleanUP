using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropFood : MonoBehaviour
{
    public GameObject food;
    GameObject foodObj;
    void OnEnable()
    {
        foodObj = Instantiate(food);
    }

    void OnDisable()
    {
        Destroy(foodObj);
    }
}
