using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth;
    public float health { get; private set; }
    [SerializeField] private SpriteRenderer spriteRend;
    [SerializeField] private float iFramesDuration;
    [SerializeField] private int numberOfFlashes;
    private bool ilnvulnerable;
    private void Awake()
    {
        health = maxHealth;

    }
    public void takeDamagePlayer(float _damage)
    {

        if (!ilnvulnerable)
        {
            health = Mathf.Clamp(health - _damage, 0, maxHealth);
            if (health == 0)
            {
                //dead
            }
            StartCoroutine(invulnerability());

        }
    }

    private IEnumerator invulnerability()
    {
        ilnvulnerable = true;
        Physics2D.IgnoreLayerCollision(7, 8, true);
        for (int i = 0; i < numberOfFlashes; i++)
        {
            spriteRend.color = new Color(1, 0, 0, 0.5f);
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
            spriteRend.color = Color.white;
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));

        }
        Physics2D.IgnoreLayerCollision(7, 8, false);
        ilnvulnerable = false;

    }
}
