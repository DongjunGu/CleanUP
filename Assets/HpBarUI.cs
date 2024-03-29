using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBarUI : MonoBehaviour
{
    public Transform target;
    public Slider hpSlider;
    public Slider easehpSlider;
    public float maxHP;
    public float hp;
    private float lerpSpeed = 0.05f;
    
    void Start()
    {
        easehpSlider.maxValue = maxHP;
        hpSlider.maxValue = hp;

        hp = maxHP;
        
    }

    
    void Update()
    {
        if (target == null) return;
        if (hpSlider.value != hp)
        {
            hpSlider.value = hp;
        }
        if (hpSlider.value != easehpSlider.value)
        {
            easehpSlider.value = Mathf.Lerp(easehpSlider.value, hp, lerpSpeed);
        }

        transform.position = Camera.main.WorldToScreenPoint(target.position);

        if(transform.position.z < 0 )
        {
            transform.position = new Vector3(0, 10000, 0);
        }
    }
    public void takeDamage(float damage)
    {
        hp -= damage; // enemy의 hp를 weapons.damage만큼 감소시킴
    }
}
