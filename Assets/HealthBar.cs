using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider hpSlider;
    public Slider easehpSlider;
    public float maxHP;
    public float hp;
    private float lerpSpeed = 0.05f;
    void Start()
    {
        hp = maxHP;
    }

    void Update()
    {
        if(hpSlider.value != hp)
        {
            hpSlider.value = hp;
        }
        if(hpSlider.value != easehpSlider.value)
        {
            easehpSlider.value = Mathf.Lerp(easehpSlider.value, hp, lerpSpeed);
        }
    }
    public void takeDamage(float damage)
    {
        hp -= damage; // enemy의 hp를 weapons.damage만큼 감소시킴
    }
}
