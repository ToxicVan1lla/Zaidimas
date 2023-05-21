using System.Collections;
using UnityEngine;

public class EnemyMove : MonoBehaviour, IDataPersistence
{
    [SerializeField] public float speed;
    [SerializeField] private int coinAmount;
    [SerializeField] public bool detectEdges;
    [SerializeField] public float attackCooldownTime;
    private float attackCooldownCounter;
    public float damage;
    public float repelForce;
    [HideInInspector] public float stopCounter;
    public bool isAlive = true;
    [HideInInspector] public GameObject player;
    [HideInInspector] public Rigidbody2D enemyBody;
    [HideInInspector] public BoxCollider2D boxCollider;
    [HideInInspector] public BoxCollider2D playerCollider;
    [HideInInspector] public Rigidbody2D playerBody;
    public float repelResistanceX, repelResistanceY;
    public bool stopsWhenHit;
    private PlayerHealth playerHealth;
    private Movement movement;
    private CoinSpawn coinSpawn;
    private Shield shield;
    public bool blocked = false;
    [SerializeField] private EnemyHealth health;
    [SerializeField] private bool respawnsInstantly = true;
    [SerializeField] private string ID;
    private bool alive = true;
    private Timer time;
    private bool teachToBlock;
    private Tutorial tutorial;
    private float CantAttackCounter;
    private bool usedTutorial;
    [ContextMenu("Generate ID")]
    private void GenerateGuid()
    {
        ID = System.Guid.NewGuid().ToString();
    }
    private DataPersistanceManager manager;
    private bool died;
    private float timeWhenKilled;
    private float timer;

    private void Awake()
    {
        time = GameObject.Find("Timer").GetComponent<Timer>();
        manager = GameObject.Find("SaveManager").GetComponent<DataPersistanceManager>();
        boxCollider = gameObject.GetComponent<BoxCollider2D>();
        enemyBody = gameObject.GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player");
        playerCollider = player.GetComponent<BoxCollider2D>();
        playerBody = player.GetComponent<Rigidbody2D>();
        movement = player.GetComponent<Movement>();
        playerHealth = player.GetComponent<PlayerHealth>();
        coinSpawn = gameObject.GetComponent<CoinSpawn>();
        shield = player.GetComponent<Shield>();
        tutorial = GameObject.Find("Tutorial").GetComponent<Tutorial>();
    }

    public void Move()
    {
        CantAttackCounter -= Time.deltaTime;
        if (!isAlive)
        {
            for(int i=0;i<coinAmount;i++)
                coinSpawn.spawnCoin(gameObject.transform);
            if(!respawnsInstantly)
            {
                died = true;
                manager.save = true;
            }
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
        attackCooldownCounter -= Time.deltaTime;

    }

    public void Attack()
    {
        if (attackCooldownCounter <= 0)
            attackCooldownCounter = attackCooldownTime;
        else return;

        bool playerisFacing = false;

        if (player.transform.position.x > transform.position.x && player.transform.localScale.x < 0)
            playerisFacing = true;
        if (player.transform.position.x < transform.position.x && player.transform.localScale.x > 0)
            playerisFacing = true;
        
        if(teachToBlock && playerisFacing && !usedTutorial)
        {
            usedTutorial = true;
            teachToBlock = false;
            tutorial.activateBlock();
            movement.dash = false;
            movement.playerAttack.isAttacking = false;
            CantAttackCounter = 2f;
            return;
        }

        if ((!shield.shieldAcitve || !playerisFacing) && CantAttackCounter <= 0)
        {
            int directionX = (playerCollider.bounds.min.x > boxCollider.bounds.min.x) ? 1 : -1;
            int directionY = (playerCollider.bounds.min.y < boxCollider.bounds.min.y) ? -1 : 1;
            if(movement.dash)
                movement.endDash();
            if(repelForce!=0)
            playerBody.velocity = Vector2.zero;

            playerBody.AddForce(repelForce * directionX * Vector2.right, ForceMode2D.Impulse);
            playerBody.AddForce(repelForce * directionY * Vector2.up, ForceMode2D.Impulse);

            playerHealth.takeDamagePlayer(damage);
        }
        else if(shield.shieldAcitve)
        {
            CantAttackCounter = 0.2f;
            bool Parried = false;
            if (shield.parryCounter > 0)
            {
                Parried = true;
                StartCoroutine(shield.Parry());
            }

            blocked = true;
            if (stopsWhenHit)
            {
                stopCounter = 0.4f;
                enemyBody.velocity = new Vector2(0, 0);
            }
            int directionX = (playerCollider.bounds.min.x > boxCollider.bounds.min.x) ? -1 : 1;
            int directionY = 1;
            enemyBody.AddForce(Mathf.Max(0, 5 - repelResistanceX * 0.5f) * directionX * Vector2.right, ForceMode2D.Impulse);
            enemyBody.AddForce(Mathf.Max(0, 2 - repelResistanceY * 0.5f) * directionY * Vector2.up, ForceMode2D.Impulse);
            if(!Parried)
            {
                playerBody.AddForce(2 * -directionX * Vector2.right, ForceMode2D.Impulse);
                shield.blocked = true;
            }
            shield.deactivateShield();

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {

        if (collision.tag == "Ground" && detectEdges && !health.gotHit)
        {
            Flip();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Shield")
            Attack();
    }

    public void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private void OnDestroy()
    {
        if(player != null)
        {
            player.GetComponent<Movement>().playerAttack.noCooldown = false;
            player.GetComponent<Movement>().playerAttack.attackSpeed = 1;
            player.GetComponent<Movement>().speed = player.GetComponent<Movement>().defaultSpeed;

            Time.timeScale = 1;

        }
    }

    public void LoadData(GameData data)
    {
        teachToBlock = data.teachBlock;
        timer = data.gameTime;
        if (data.enemies.ContainsKey(ID))
            timeWhenKilled = data.enemies[ID];
        if (data.enemies.ContainsKey(ID) && timer - timeWhenKilled < 15 && alive && boxCollider!=null)
        {
            Destroy(gameObject);
            alive = false;
        }
    }

    public void SaveData(ref GameData data)
    {
        if(died)
        {
            if (data.enemies.ContainsKey(ID))
                data.enemies.Remove(ID);
            data.enemies.Add(ID, time.timer);
        }
    }

}
