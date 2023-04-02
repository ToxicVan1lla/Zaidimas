using System.Collections;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    private float health;

    [SerializeField] private float maxHealth;
    private SpriteRenderer spriteRend;

    private void Awake()
    {
        health = maxHealth;
        spriteRend = GetComponent<SpriteRenderer>();
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
