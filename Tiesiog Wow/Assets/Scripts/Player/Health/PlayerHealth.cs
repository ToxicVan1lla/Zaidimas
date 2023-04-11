using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerHealth : MonoBehaviour
{
    [SerializeField] public float maxHealth;
    public float health;
    [SerializeField] private SpriteRenderer spriteRend;
    [SerializeField] private float iFramesDuration;
    [SerializeField] private int numberOfFlashes;
    [SerializeField] private KeepData keepData;
    private bool invulnerable;
    private Movement movement;
    void Start()
    {
        movement = gameObject.GetComponent<Movement>();
        Physics2D.IgnoreLayerCollision(7, 8, false);
        health = maxHealth;
        health = keepData.health;
        invulnerable = false;

    }
    public void takeDamagePlayer(float _damage)
    {
        if (!invulnerable)
        {
            health = Mathf.Clamp(health - _damage, 0, maxHealth);
            if (health == 0)
            {
                keepData.positionX = keepData.checkpointX;
                keepData.positionY = keepData.checkpointY;
                keepData.health = maxHealth;
                keepData.enteredRoom = false;
                keepData.graveValue = keepData.coinAmount;
                keepData.graveActive = true;
                keepData.graveScene = SceneManager.GetActiveScene().name;
                keepData.graveX = movement.lastGroundedX;
                keepData.graveY = movement.lastGroundedY;
                keepData.coinAmount = 0;
                SceneManager.LoadScene(keepData.checkpointSceneName);

            }
            else
                StartCoroutine(invulnerability());

        }
    }

    private IEnumerator invulnerability()
    {
        invulnerable = true;
        Physics2D.IgnoreLayerCollision(7, 8, true);
        for (int i = 0; i < numberOfFlashes; i++)
        {
            spriteRend.color = new Color(1, 0, 0, 0.5f);
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
            spriteRend.color = Color.white;
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));

        }
        Physics2D.IgnoreLayerCollision(7, 8, false);
        invulnerable = false;

    }
}
