using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapons : MonoBehaviour
{
    public enum Type { basicAttack };
    public Type type;
    public int damage;
    public float attackSpeed;
    public BoxCollider basicAttack_Range;
    public TrailRenderer trailEffect;
    public GameObject hitEffect;

    public void Attack()
    {
        if(type == Type.basicAttack)
        {
            StopCoroutine("Wipe");
            StartCoroutine("Wipe");
        }
    }

    IEnumerator Wipe()
    {
        yield return new WaitForSeconds(0.1f);
        basicAttack_Range.enabled = true;
        trailEffect.enabled = true;

        yield return new WaitForSeconds(0.3f);
        basicAttack_Range.enabled = false;

        yield return new WaitForSeconds(0.3f);
        trailEffect.enabled = false;
    }
}
