using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private Rigidbody2D body;
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
        body = gameObject.GetComponent<Rigidbody2D>();
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
                Vector2 direction = (enemy.GetComponent<Rigidbody2D>().transform.position - body.transform.position).normalized;
                enemy.GetComponent<PatrolMovement>().stopCounter = 0.5f;
                enemy.GetComponent<Rigidbody2D>().AddForce(repelForce * direction, ForceMode2D.Impulse);
                enemy.GetComponent<EnemyHealth>().takeDamageEnemie(damage);
            }
            attackRateCounter = 0;
        }
        else
            attackRateCounter += Time.deltaTime;


    }
  
}
