using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    private float health;

    [SerializeField] private float maxHealth;
    [SerializeField] private SpriteRenderer spriteRend;
    [SerializeField] PatrolMovement patrolMovement;

    private void Awake()
    {
        health = maxHealth;

    }
    public float takeDamageEnemie(float _damage)
    {
        health = Mathf.Clamp(health - _damage, 0, maxHealth);

        StartCoroutine(Flash());
        return health;
    }

    private IEnumerator Flash()
    {
        spriteRend.color = new Color(1, 0, 0, 0.5f);
        yield return new WaitForSeconds(0.2f);
        spriteRend.color = Color.white;
    }
}
