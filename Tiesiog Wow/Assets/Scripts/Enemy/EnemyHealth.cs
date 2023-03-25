using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    private float health;

    [SerializeField] private float maxHealth;
    [SerializeField] private SpriteRenderer spriteRend;

    private void Awake()
    {
        health = maxHealth;

    }
    public void takeDamageEnemie(float _damage)
    {
        health = Mathf.Clamp(health - _damage, 0, maxHealth);
        if (health == 0)
        {
            //dead
        }
        StartCoroutine(Flash());
    }

    private IEnumerator Flash()
    {
        spriteRend.color = new Color(1, 0, 0, 0.5f);
        yield return new WaitForSeconds(0.2f);
        spriteRend.color = Color.white;
    }
}
