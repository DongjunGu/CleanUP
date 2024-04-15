using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonitorNoise : MonoBehaviour
{
    void Update()
    {
        float randomValue = Random.value;
        Material material = transform.GetComponent<Renderer>().sharedMaterial;
        material.SetFloat("_SmoothnessTextureChannel", randomValue);
        material.SetFloat("_Metallic", randomValue);
    }
}

