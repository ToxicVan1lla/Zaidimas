using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    [SerializeField] public float speed;
    [SerializeField] private int coinAmount;
    [SerializeField] public bool detectEdges;
    public float damage;
    public float repelForce;
    [HideInInspector] public float stopCounter;
    public bool isAlive = true;
    private GameObject player;
    [HideInInspector] public Rigidbody2D enemyBody;
    [HideInInspector] public BoxCollider2D boxCollider;
    [HideInInspector] public BoxCollider2D playerCollider;
    [HideInInspector] public Rigidbody2D playerBody;
    public float repelResistance;
    public bool stopsWhenHit;
    private PlayerHealth playerHealth;
    private Movement movement;
    private CoinSpawn coinSpawn;

    private void Awake()
    {
        boxCollider = gameObject.GetComponent<BoxCollider2D>();
        enemyBody = gameObject.GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player");
        playerCollider = player.GetComponent<BoxCollider2D>();
        playerBody = player.GetComponent<Rigidbody2D>();
        movement = player.GetComponent<Movement>();
        playerHealth = player.GetComponent<PlayerHealth>();
        coinSpawn = gameObject.GetComponent<CoinSpawn>();
    }

    public void Move()
    {
        if (!isAlive)
        {
            for(int i=0;i<coinAmount;i++)
                coinSpawn.spawnCoin(gameObject.transform);

            Destroy(gameObject);
        }

        if (stopCounter > 0)
        {
            stopCounter -= Time.deltaTime;
        }
        else
        {
            enemyBody.velocity = new Vector2(Mathf.Sign(transform.localScale.x) * speed, enemyBody.velocity.y);

        }

    }

    public void Attack()
    {
        int directionX = (playerCollider.bounds.min.x > boxCollider.bounds.min.x) ? 1 : -1;
        int directionY = (playerCollider.bounds.min.y < boxCollider.bounds.min.y) ? -1 : 1;
        if(movement.dash)
            movement.endDash();
        playerBody.velocity = Vector2.zero;

        playerBody.AddForce(repelForce * directionX * Vector2.right, ForceMode2D.Impulse);
        playerBody.AddForce(repelForce * directionY * Vector2.up, ForceMode2D.Impulse);

        playerHealth.takeDamagePlayer(damage);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {

        if (collision.tag == "Ground" && detectEdges)
        {
            Flip();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ( collision.tag == "Wall" && detectEdges)
        {
            Flip();
        }
    }
    public void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

}
