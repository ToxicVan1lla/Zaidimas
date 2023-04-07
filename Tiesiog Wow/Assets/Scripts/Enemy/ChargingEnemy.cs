using UnityEngine;
using System.Collections;


public class ChargingEnemy : EnemyMove
{
    [SerializeField] private LayerMask Enemy;
    [SerializeField] private float range;
    [SerializeField] private float ChargeTime;
    [SerializeField] private float colliderDistance;
    [SerializeField] private float rollSpeed;
    [SerializeField] private float rollDuration;
    private float untilCharge;
    private CircleCollider2D circleCollider;
    private Rigidbody2D rb;
    private Coroutine roll;
    private bool isRollingForwards;
    [SerializeField] private Animator animator;
    private bool isRolling = false;
    private bool finishedRolling;

    private float startPossition;
    private EnemyHealth enemyHealth;
    private bool turnAround = false;

    void Start()
    {
        enemyHealth = gameObject.GetComponent<EnemyHealth>();
        circleCollider = gameObject.GetComponent<CircleCollider2D>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        Physics2D.IgnoreLayerCollision(8, 8, true);
        circleCollider.enabled = false;

    }

    private void Update()
    {
        if (enemyHealth.gotHit && isRollingForwards)
        {
            turnAround = true;
            isRollingForwards = false;
            StopCoroutine(roll);
            StartCoroutine(RollBack());
            stopCounter = 0.2f;
        }
        else
            turnAround = false;

        Move();
        if (boxCollider.IsTouching(playerCollider) || circleCollider.IsTouching(playerCollider))
        {
            Attack();
            if(isRollingForwards)
            {
                StopCoroutine(roll);
                StartCoroutine(RollBack());
                isRollingForwards = false;
            }
        }
        untilCharge -= Time.deltaTime;
        if(!isRolling && untilCharge < 0 && seesPlayer())
        {

            roll = StartCoroutine(RollForwards());
        }

    }
    
    private IEnumerator RollForwards()
    {
        startPossition = rb.position.x;
        isRolling = true;        
        animator.SetBool("Idle", false);
        animator.SetTrigger("Transform");
        do
        {
          yield return null;
        } while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1);

        isRollingForwards = true;
        circleCollider.enabled = true;
        boxCollider.enabled = false;
        speed = rollSpeed;
        animator.SetBool("Roll", true);
        yield return new WaitForSeconds(rollDuration);
        isRollingForwards = false;
        StartCoroutine(RollBack());
    }
    private IEnumerator RollBack()
    {
        if (!turnAround)
        {
            speed = 0;
            animator.SetBool("Roll", false);
            animator.SetTrigger("TransformToStanding");
            do
            {
                yield return null;
            } while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1);

            animator.SetTrigger("Attack");
            do
            {
                yield return null;
            } while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1);

            animator.SetTrigger("Transform");

            do
            {
                yield return null;
            } while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1);
        }
        animator.SetBool("Roll", true);
        speed = -rollSpeed;
        finishedRolling = false;
        while(!finishedRolling)
        {
            if (rb.transform.localScale.x > 0 && rb.position.x <= startPossition)
                finishedRolling = true;
            else if (rb.transform.localScale.x < 0 && rb.position.x >= startPossition)
                finishedRolling = true;

            yield return null;
        }

        animator.SetTrigger("TransformToStanding");

        do
        {
            yield return null;
        } while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1);
        animator.SetBool("Roll", false);
        animator.SetBool("Idle", true);

        speed = 0;
        boxCollider.enabled = true;
        circleCollider.enabled = false;
        untilCharge = ChargeTime;
        isRolling = false;
    }

    private bool seesPlayer()
    {
        RaycastHit2D hit = Physics2D.Raycast(boxCollider.bounds.center, new Vector2(1 * Mathf.Sign(rb.transform.localScale.x), 0), range, Enemy);
        if (hit.collider != null && hit.collider.CompareTag("Player"))
            return true;
        else
            return false;

    }
}
