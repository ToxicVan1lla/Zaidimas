using UnityEngine;

public class PatrolMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private BoxCollider2D playerCollider;
    private Rigidbody2D body;
    private BoxCollider2D boxCollider;
    [SerializeField] private float damage;
    [SerializeField] private float repelForce;
    [SerializeField] private Rigidbody2D playerBody;
    [SerializeField] private PlayerHealth playerHealth;
    public float stopCounter;

    void Start()
    {
        Physics2D.IgnoreLayerCollision(8, 8, true);
        body = gameObject.GetComponent<Rigidbody2D>();
        boxCollider = gameObject.GetComponent<BoxCollider2D>();        
    }

    // Update is called once per frame
    void Update()
    {
        if (stopCounter > 0)
            stopCounter -= Time.deltaTime;
        else
            body.velocity = new Vector2(Mathf.Sign(transform.localScale.x) * speed, body.velocity.y);
        if (boxCollider.IsTouching(playerCollider))
        {
            Vector2 direction = (playerBody.transform.position - body.transform.position).normalized;
            playerBody.AddForce(repelForce * direction, ForceMode2D.Impulse);
            playerHealth.takeDamagePlayer(damage);
        }
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
        if(collision.tag == "Wall")
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
