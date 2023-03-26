using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private new BoxCollider2D collider;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float repelForce;
    [SerializeField] private float damage;
    [SerializeField] private float attackRange;
    [SerializeField] private LayerMask enemys;
    [SerializeField] EnemyHealth enemyHealth;
    [SerializeField] PatrolMovement patrolMovement;
    [SerializeField] private float attackRateTime;
    private float attackRateCounter;

    private void Start()
    {
        collider = gameObject.GetComponent<BoxCollider2D>();
    }

    private bool attack;
    private void Update()
    {
        attack = Input.GetButtonDown("Fire1");

        if (attack && attackRateCounter > attackRateTime)
        {
            Collider2D[] hitEnemys = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemys);
            foreach (Collider2D enemy in hitEnemys)
            {
                enemy.GetComponent<EnemyMove>().stopCounter = 0.5f;
                int directionX = (collider.bounds.min.x > enemy.GetComponent<BoxCollider2D>().bounds.min.x)? -1 : 1;
                int directionY = (collider.bounds.min.y > enemy.GetComponent<BoxCollider2D>().bounds.min.y) ? -1 : 1;
                enemy.GetComponent<Rigidbody2D>().AddForce(repelForce * directionX * Vector2.right, ForceMode2D.Impulse);
                enemy.GetComponent<Rigidbody2D>().AddForce(repelForce * directionY * Vector2.up, ForceMode2D.Impulse);


                if (enemy.GetComponent<EnemyHealth>().takeDamageEnemie(damage) == 0)
                    enemy.GetComponent<EnemyMove>().isAlive = false;
            }
            attackRateCounter = 0;
        }
        else
            attackRateCounter += Time.deltaTime;


    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

}
