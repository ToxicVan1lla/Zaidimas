using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    [SerializeField] private float speed;
    public Rigidbody2D body;
    public float damage;
    public float repelForce;
    public float stopCounter;
    public bool isAlive = true;
    [SerializeField] public BoxCollider2D playerCollider;
    [SerializeField] public BoxCollider2D boxCollider;
    [SerializeField] private Rigidbody2D playerBody;
    [SerializeField] private PlayerHealth playerHealth;

    public void Move()
    {
        if (!isAlive)
        {
            Destroy(gameObject);
        }

        if (stopCounter > 0)
        {
            stopCounter -= Time.deltaTime;
        }
        else
            body.velocity = new Vector2(Mathf.Sign(transform.localScale.x) * speed, body.velocity.y);

    }

    public void Attack()
    {
        Vector2 direction = (playerBody.transform.position - body.transform.position).normalized;
        playerBody.AddForce(repelForce * direction, ForceMode2D.Impulse);
        playerHealth.takeDamagePlayer(damage);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {

        if (collision.tag == "Ground")
        {
            Flip();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Wall")
        {
            Flip();
        }
    }
    private void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

}
